/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Offers
{
    public class Offer : IDisposable
    {
        public VehicleModel Vehicle { get; }
        public ItemModel ItemModel { get; }
        public BuildingModel Building { get; }

        public decimal Money { get; }

        //Ten co wysyłał oferte
        public CharacterEntity Sender { get; }

        //Ten co odbiera oferte
        public CharacterEntity Getter { get; }

        //Oferta z przedmiotem
        public Offer(CharacterEntity sender, CharacterEntity getter, ItemModel itemModel, decimal moneyCount)
        {
            ItemModel = itemModel;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta z pojazdem
        public Offer(CharacterEntity sender, CharacterEntity getter, VehicleModel vehicle, decimal moneyCount)
        {
            Vehicle = vehicle;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta z budynkiem
        public Offer(CharacterEntity sender, CharacterEntity getter, BuildingModel building, decimal moneyCount)
        {
            Building = building;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta bez niczego, np. taxi, naprawa, itp.
        public Offer(CharacterEntity sender, CharacterEntity getter, decimal moneyCount, Action<CharacterEntity> action, bool moneyToGroup)
        {
            _action = action;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
            _moneyToGroup = moneyToGroup;
        }

        private Action<CharacterEntity> _action;
        //Determinuje czy gotówka ma iść do kieszeni gracza czy do grupy
        private bool _moneyToGroup;

        public void Trade(bool bank)
        {
            if (Getter.HasMoney(Money, bank))
            {
                if (_moneyToGroup && Sender.OnDutyGroup == null)
                {
                    Sender.SendInfo("Musisz znajdować się na służbie grupy.");
                    return;
                }

                if (ItemModel != null)
                {
                    ChatScript.SendMessageToNearbyPlayers(Sender,
                        $"{Sender.FormatName} podaje przedmiot {Getter.FormatName}",
                        ChatMessageType.ServerMe);

                    ItemModel.Character = Getter.DbModel;

                    using (ItemsRepository repository = new ItemsRepository())
                    {
                        repository.Update(ItemModel);
                        repository.Save();
                    }
                }
                else if (Vehicle != null)
                {
                    ChatScript.SendMessageToNearbyPlayers(Sender,
                        $"{Sender.FormatName} podaje klucze {Getter.FormatName}",
                        ChatMessageType.ServerMe);

                    Vehicle.Character = Getter.DbModel;

                    using (VehiclesRepository repository = new VehiclesRepository())
                    {
                        repository.Update(Vehicle);
                        repository.Save();
                    }
                }
                else if (Building != null)
                {
                    ChatScript.SendMessageToNearbyPlayers(Sender,
                        $"{Sender.FormatName} podaje klucze {Getter.FormatName}",
                        ChatMessageType.ServerMe);

                    Building.Character = Getter.DbModel;

                    using (BuildingsRepository repository = new BuildingsRepository())
                    {
                        repository.Update(Building);
                        repository.Save();
                    }
                }

                if (_moneyToGroup)
                {
                    Sender.OnDutyGroup.AddMoney(Money);
                }
                else
                {
                    Sender.AddMoney(Money, bank);
                }

                Getter.RemoveMoney(Money, bank);

                _action?.Invoke(Getter);
            }
            else
            {
                
                Getter.SendWarning(bank
                    ? "Nie posiadasz wystarczającej ilości środków na karcie"
                    : "Nie posiadasz wystarczającej ilości gotówki.");
                Sender.SendError("Wymiana zakończona niepowodzeniem.");
            }
        }

        public void ShowWindow(List<string> dataSource)
        {
            NAPI.ClientEvent.TriggerClientEvent(Getter.AccountEntity.Client, "ShowOfferCef", dataSource);
        }

        public void Dispose()
        {
            Getter.PendingOffer = null;
        }
    }
}
