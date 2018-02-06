/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Core.Scripts;

namespace Serverside.Offers
{
    public class Offer : IDisposable
    {
        public VehicleModel Vehicle { get; }
        public ItemModel ItemModel { get; }
        public BuildingModel Building { get; }

        public decimal Money { get; }

        //Ten co wysyłał oferte
        public Client Sender { get; }

        //Ten co odbiera oferte
        public Client Getter { get; }

        //Oferta z przedmiotem
        public Offer(Client sender, Client getter, ItemModel itemModel, decimal moneyCount)
        {
            ItemModel = itemModel;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta z pojazdem
        public Offer(Client sender, Client getter, VehicleModel vehicle, decimal moneyCount)
        {
            Vehicle = vehicle;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta z budynkiem
        public Offer(Client sender, Client getter, BuildingModel building, decimal moneyCount)
        {
            Building = building;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
        }

        //Oferta bez niczego, np. taxi, naprawa, itp.
        public Offer(Client sender, Client getter, decimal moneyCount, Action<Client> action, bool moneyToGroup)
        {
            _action = action;
            Money = moneyCount;
            Sender = sender;
            Getter = getter;
            _moneyToGroup = moneyToGroup;
        }

        private Action<Client> _action;
        //Determinuje czy gotówka ma iść do kieszeni gracza czy do grupy
        private bool _moneyToGroup;

        public void Trade(bool bank)
        {
            if (Getter.HasMoney(Money, bank))
            {
                if (ItemModel != null)
                {
                    ChatScript.SendMessageToNearbyPlayers(Sender,
                        $"{Sender.GetAccountEntity().CharacterEntity.FormatName} podaje przedmiot {Getter.GetAccountEntity().CharacterEntity.FormatName}",
                        ChatMessageType.ServerMe);

                    ItemModel.Character = Getter.GetAccountEntity().CharacterEntity.DbModel;

                    using (ItemsRepository repository = new ItemsRepository())
                    {
                        repository.Update(ItemModel);
                        repository.Save();
                    }
                }
                else if (Vehicle != null)
                {
                    Vehicle.Character = Getter.GetAccountEntity().CharacterEntity.DbModel;

                    using (VehiclesRepository repository = new VehiclesRepository())
                    {
                        repository.Update(Vehicle);
                        repository.Save();
                    }
                }
                else if (Building != null)
                {
                    Building.Character = Getter.GetAccountEntity().CharacterEntity.DbModel;
                    using (BuildingsRepository repository = new BuildingsRepository())
                    {
                        repository.Update(Building);
                        repository.Save();
                    }
                }

                if (_moneyToGroup)
                {
                    Sender.GetAccountEntity().CharacterEntity.OnDutyGroup.AddMoney(Money);
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
                Getter.Notify(bank
                    ? "Nie posiadasz wystarczającej ilości środków na karcie"
                    : "Nie posiadasz wystarczającej ilości gotówki.");
                Sender.Notify("Wymiana zakończona niepowodzeniem.");
            }
        }

        public void ShowWindow(List<string> dataSource)
        {
            NAPI.ClientEvent.TriggerClientEvent(Getter, "ShowOfferCef", dataSource);
        }

        public void Dispose()
        {
            Getter.ResetData("Offer");
        }
    }
}
