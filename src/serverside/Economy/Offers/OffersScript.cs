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
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Economy.Offers
{
    public class OffersScript : Script
    {
        [RemoteEvent(RemoteEvents.OnPlayerCancelOffer)]
        public void OnPLayerCancelOfferHandler(Client sender, string eventName, object[] arguments)
        {
            Offer offer = sender.GetData("Offer");

            offer.Sender.SendWarning($"Gracz {offer.Getter.FormatName} odrzucił twoją ofertę.");
            offer.Getter.SendWarning($"Odrzuciłeś ofertę gracza { offer.Sender.FormatName}");
            offer.Dispose();
        }

        [RemoteEvent(RemoteEvents.OnPlayerPayOffer)]
        public void OnPlayerPayOfferHandler(Client sender, string eventName, object[] arguments)
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
      

        #region Komendy
        [Command("o", "~y~UŻYJ: ~w~ /o [id] [typ] [cena] (indeks)")]
        public void OfferItem(Client sender, int id, OfferType type, decimal safeMoneyCount, int index = -1)
        {
            if (id.Equals(sender.GetAccountEntity().ServerId))
            {
                sender.SendError("Nie możesz oferować przedmiotu samemu sobie.");
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
                        sender.SendError("Nie posiadasz przedmiotu o takim indeksie.");
                        return;
                    }

                    ItemModel item = items[index];

                    if (sender.GetAccountEntity().CharacterEntity.ItemsInUse.Any(i => i.Id == item.Id))
                    {
                        sender.SendError("Nie możesz używać przedmiotu podczas oferowania.");
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
                            sender.SendError("Nie można sprzedać budynku przepisanego pod grupę.");
                            return;
                        }

                        if (building.DbModel.Character.Id != sender.GetAccountEntity().CharacterEntity.DbModel
                                .Id)
                        {
                            sender.SendError("Nie jesteś właścicielem tego budynku.");
                            return;
                        }

                        offer = new Offer(senderCharacter, getterCharacter, building.DbModel, safeMoneyCount);
                    }
                    else
                    {
                        sender.SendError("Aby oferować budynek musisz znajdować się w markerze bądź środku budynku");
                    }
                }
                //Tutaj są oferty wymagające uprawnień grupowych
                else if (type == OfferType.IdCard)
                {
                    GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
                    if (group == null) return;
                    if (group.DbModel.GroupType != GroupType.CityHall || !((CityHall)group).CanPlayerGiveIdCard(sender.GetAccountEntity()))
                    {
                        sender.SendWarning("Twoja grupa, bądź postać nie posiada uprawnień do wydawania dowodu osobistego.");
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
                        sender.SendInfo("Twoja grupa, bądź postać nie posiada uprawnień do wydawania prawa jazdy.");
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
                sender.SendInfo("Twoja oferta została wysłana.");
                offer.ShowWindow(cefList);
            }
        }
    }
    #endregion
}