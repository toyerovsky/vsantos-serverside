/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Building;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Economy.Offers
{
    public class OffersScript : Script
    {
        public void API_OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "OnPlayerCancelOffer")
            {
                Offer offer = sender.GetData("Offer");

                offer.Sender.SendWarning($"Gracz {offer.Getter.FormatName} odrzucił twoją ofertę.");
                offer.Getter.SendWarning($"Odrzuciłeś ofertę gracza { offer.Sender.FormatName}");
                offer.Dispose();
            }
            else if (eventName == "OnPlayerPayOffer")
            {
                Offer offer = sender.GetData("Offer");

                if (offer.Sender.AccountEntity.Client.Position.DistanceTo(offer.Getter.AccountEntity.Client.Position) <= 10)
                {
                    //Trzeba nadać to pole przed wykonaniem oferty
                    //FixMe
                    //offer.Bank = Convert.ToBoolean(arguments[0]);
                    //offer.Trade();
                }
                else
                {
                    offer.Sender.SendError("Osoba do której wysyłasz ofertę znajduje się za daleko.");
                    offer.Getter.SendError("Znajdujesz się za daleko od osoby która wysłała ofertę.");
                }
                offer.Dispose();
            }
        }

        #region Komendy
        [Command("o", "~y~UŻYJ: ~w~ /o [id] [typ] [cena] (indeks)")]
        public void OfferItem(Client sender, int id, OfferType type, decimal safeMoneyCount, int index = -1)
        {
            if (id.Equals(sender.GetAccountEntity().ServerId))
            {
                sender.Notify("Nie możesz oferować przedmiotu samemu sobie.", NotificationType.Error);
                return;
            }

            Offer offer = null;
            if (NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Any(x => x.GetAccountEntity().ServerId == id))
            {
                CharacterEntity senderCharacter = sender.GetAccountEntity().CharacterEntity;
                CharacterEntity getterCharacter = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender)
                    .Find(x => x.GetAccountEntity().ServerId == id).GetAccountEntity().CharacterEntity;

                if (type == OfferType.Item && index != -1)
                {
                    List<ItemModel> items = sender.GetAccountEntity().CharacterEntity.DbModel.Items.ToList();

                    //Tutaj sprawdzamy czy gracz posiada taki numer na liście. Numerujemy od 0 więc items.Count - 1
                    if (index > items.Count - 1 || index < 0)
                    {
                        sender.Notify("Nie posiadasz przedmiotu o takim indeksie.", NotificationType.Error);
                        return;
                    }

                    ItemModel item = items[index];

                    if (sender.GetAccountEntity().CharacterEntity.ItemsInUse.Any(i => i.Id == item.Id))
                    {
                        sender.Notify("Nie możesz używać przedmiotu podczas oferowania.", NotificationType.Error);
                        return;
                    }

                    offer = new Offer(senderCharacter, getterCharacter, item, safeMoneyCount);
                }
                else if (type == OfferType.Vehicle)
                {
                    VehicleEntity vehicle = EntityHelper.GetVehicle(sender.Vehicle);
                    if (vehicle == null) return;

                    offer = new Offer(senderCharacter, getterCharacter, vehicle.DbModel, safeMoneyCount);
                }
                else if (type == OfferType.Building)
                {
                    if (sender.GetAccountEntity().CharacterEntity.CurrentBuilding != null || sender.HasData("CurrentDoors"))
                    {
                        BuildingEntity building = sender.HasData("CurrentDoors")
                            ? sender.GetData("CurrentDoors")
                            : sender.GetAccountEntity().CharacterEntity.CurrentBuilding;

                        if (building.DbModel.Group != null)
                        {
                            sender.Notify("Nie można sprzedać budynku przepisanego pod grupę.", NotificationType.Error);
                            return;
                        }

                        if (building.DbModel.Character.Id != sender.GetAccountEntity().CharacterEntity.DbModel
                                .Id)
                        {
                            sender.Notify("Nie jesteś właścicielem tego budynku.", NotificationType.Error);
                            return;
                        }

                        offer = new Offer(senderCharacter, getterCharacter, building.DbModel, safeMoneyCount);
                    }
                    else
                    {
                        sender.Notify("Aby oferować budynek musisz znajdować się w markerze bądź środku budynku", NotificationType.Error);
                    }
                }
                //Tutaj są oferty wymagające uprawnień grupowych
                else if (type == OfferType.IdCard)
                {
                    GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
                    if (group == null) return;
                    if (group.DbModel.GroupType != GroupType.CityHall || !((CityHall)group).CanPlayerGiveIdCard(sender.GetAccountEntity()))
                    {
                        sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do wydawania dowodu osobistego.", NotificationType.Error);
                        return;
                    }
                    offer = new Offer(senderCharacter, getterCharacter, safeMoneyCount, c => OfferActions.GiveIdCard(getterCharacter), true);
                }
                else if (type == OfferType.DrivingLicense)
                {
                    GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
                    if (group == null) return;
                    if (group.DbModel.GroupType != GroupType.CityHall || !((CityHall)group).CanPlayerGiveDrivingLicense(sender.GetAccountEntity()))
                    {
                        sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do wydawania prawa jazdy." ,NotificationType.Info);
                        return;
                    }
                    offer = new Offer(senderCharacter, getterCharacter, safeMoneyCount, c => OfferActions.GiveDrivingLicense(getterCharacter), true);
                }

                if (offer != null) getterCharacter.PendingOffer = offer;
            }

            if (offer != null)
            {
                List<string> cefList = new List<string>
                {
                    sender.Name,
                    type.ToString(),
                    offer.Money.ToString(CultureInfo.InvariantCulture)
                };
                sender.Notify("Twoja oferta została wysłana.", NotificationType.Info);
                offer.ShowWindow(cefList);
            }
        }
    }
    #endregion
}