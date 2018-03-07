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

                offer.Sender.Notify($"Gracz {offer.Getter.GetAccountEntity().CharacterEntity.FormatName} odrzucił twoją ofertę.");
                offer.Getter.Notify($"Odrzuciłeś ofertę gracza { offer.Sender.GetAccountEntity().CharacterEntity.FormatName}");
                offer.Dispose();
            }
            else if (eventName == "OnPlayerPayOffer")
            {
                Offer offer = sender.GetData("Offer");

                if (offer.Sender.Position.DistanceTo(offer.Getter.Position) <= 10)
                {
                    //Trzeba nadać to pole przed wykonaniem oferty
                    //FixMe
                    //offer.Bank = Convert.ToBoolean(arguments[0]);
                    //offer.Trade();
                }
                else
                {
                    offer.Sender.Notify("Osoba do której wysyłasz ofertę znajduje się za daleko.");
                    offer.Getter.Notify("Znajdujesz się za daleko od osoby która wysłała ofertę.");
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
                sender.Notify("Nie możesz oferować przedmiotu samemu sobie.");
                return;
            }

            Offer offer = null;
            if (NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Any(x => x.GetAccountEntity().ServerId == id))
            {
                Client getter = NAPI.Player.GetPlayersInRadiusOfPlayer(6f, sender).Find(x => x.GetAccountEntity().ServerId == id);
                if (type == OfferType.Item && index != -1)
                {
                    List<ItemModel> items = sender.GetAccountEntity().CharacterEntity.DbModel.Items.ToList();

                    //Tutaj sprawdzamy czy gracz posiada taki numer na liście. Numerujemy od 0 więc items.Count - 1
                    if (index > items.Count - 1 || index < 0)
                    {
                        sender.Notify("Nie posiadasz przedmiotu o takim indeksie.");
                        return;
                    }

                    ItemModel item = items[index];

                    if (sender.GetAccountEntity().CharacterEntity.ItemsInUse.Any(i => i.Id == item.Id))
                    {
                        sender.Notify("Nie możesz używać przedmiotu podczas oferowania.");
                        return;
                    }

                    offer = new Offer(sender, getter, item, safeMoneyCount);
                }
                else if (type == OfferType.Vehicle)
                {
                    VehicleEntity vehicle = EntityHelper.GetVehicle(sender.Vehicle);
                    if (vehicle == null) return;

                    offer = new Offer(sender, getter, vehicle.DbModel, safeMoneyCount);
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
                            sender.Notify("Nie można sprzedać budynku przepisanego pod grupę.");
                            return;
                        }

                        if (building.DbModel.Character.Id != sender.GetAccountEntity().CharacterEntity.DbModel
                                .Id)
                        {
                            sender.Notify("Nie jesteś właścicielem tego budynku.");
                            return;
                        }

                        offer = new Offer(sender, getter, building.DbModel, safeMoneyCount);
                    }
                    else
                    {
                        sender.Notify("Aby oferować budynek musisz znajdować się w markerze bądź środku budynku");
                    }
                }
                //Tutaj są oferty wymagające uprawnień grupowych
                else if (type == OfferType.IdCard)
                {
                    GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
                    if (group == null) return;
                    if (group.DbModel.GroupType != GroupType.CityHall || !((CityHall)group).CanPlayerGiveIdCard(sender.GetAccountEntity()))
                    {
                        sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do wydawania dowodu osobistego.");
                        return;
                    }
                    offer = new Offer(sender, getter, safeMoneyCount, c => OfferActions.GiveIdCard(getter), true);
                }
                else if (type == OfferType.DrivingLicense)
                {
                    GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
                    if (group == null) return;
                    if (group.DbModel.GroupType != GroupType.CityHall || !((CityHall)group).CanPlayerGiveDrivingLicense(sender.GetAccountEntity()))
                    {
                        sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do wydawania prawa jazdy.");
                        return;
                    }
                    offer = new Offer(sender, getter, safeMoneyCount, c => OfferActions.GiveDrivingLicense(getter), true);
                }

                if (offer != null) getter.SetData("Offer", offer);
            }

            if (offer != null)
            {
                List<string> cefList = new List<string>
                {
                    sender.Name,
                    type.ToString(),
                    offer.Money.ToString(CultureInfo.InvariantCulture)
                };
                sender.Notify("Twoja oferta została wysłana.");
                offer.ShowWindow(cefList);
            }
        }
    }
    #endregion
}