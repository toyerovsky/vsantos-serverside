/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database.Models.CrimeBot;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Entities.Core.Vehicle;
using VRP.Serverside.Entities.Peds.CrimeBot.Models;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Peds.CrimeBot
{
    public class CrimePedEntity : PedEntity
    {
        private GroupEntity Group { get; }
        private CrimeBotModel DbModel { get; }

        private List<CrimeBotItem> Items { get; set; } = new List<CrimeBotItem>();

        private ColShape BotShape { get; set; }
        private AccountEntity Player { get; }
        private VehicleEntity Vehicle { get; set; }
        private FullPosition VehiclePosition { get; }

        public CrimePedEntity(AccountEntity player, CrimeGroup group, FullPosition vehiclePosition,
            string name, PedHash hash, FullPosition position) : base(name, hash, position)
        {
            Player = player;
            Group = group;
            VehiclePosition = vehiclePosition;

            using (CrimeBotsRepository repository = new CrimeBotsRepository())
                DbModel = repository.Get(crimeBot => crimeBot.GroupModel.Id == group.Id);

            List<PropertyInfo> properties = new List<PropertyInfo> { null };
            properties.AddRange(typeof(CrimeBotModel).GetProperties()
                .Where(f => f.GetValue(DbModel) != null && (f.PropertyType == typeof(int?) || f.PropertyType == typeof(decimal?))));

            if (properties.Count % 3 != 1)
            {
                player.Client.SendError($"Konfiguracja bota grupy {Group} jest nieporawna, skontaktuj się z administratorem.");
                return;
            }

            for (int i = 0; i < properties.Count; i += 3)
            {
                if (i == 0) continue;

                Tuple<string, ItemEntityType> info = Constant.Items.GetCrimeBotItemName(properties[i - 2].Name);
                Items.Add(new CrimeBotItem(info.Item1, ((decimal?)properties[i - 2].GetValue(DbModel)).Value, ((int?)properties[i - 1].GetValue(DbModel)).Value, ((int?)properties[i].GetValue(DbModel)).Value, info.Item2, properties[i - 1].Name));

            }
            Items.ForEach(
                x => NAPI.Chat.SendChatMessageToPlayer(Player.Client, $"Nazwa {x.Name} Koszt {x.Cost}, Ilość {x.Count}, Pole {x.DatabaseField}"));
        }

        public override void Spawn()
        {
            base.Spawn();

            Vehicle = VehicleEntity.Create(VehiclePosition,
                NAPI.Util.VehicleNameToModel(DbModel.VehicleModel), DbModel.Name, 0, DbModel.CreatorId, new Color(0, 0, 0), new Color(0, 0, 0));
            Vehicle.GameVehicle.OpenDoor(5);
            BotHandle.PlayScenario("WORLD_HUMAN_SMOKING");

            BotShape = NAPI.ColShape.CreateCylinderColShape(BotHandle.Position, 3f, 3f);
            BotShape.OnEntityEnterColShape += BotShape_OnEntityEnterColShape;

            Timer timer = new Timer(1800000);
            timer.Start();
            timer.Elapsed += (o, e) =>
            {
                Dispose(true);
                timer.Dispose();
            };
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            ////args 0 to string JSON z js który mówi co gracz kupił
            //if (eventName == "OnCrimeBotBought")
            //{
            //    List<CrimeBotItem> items =
            //        JsonConvert.DeserializeObject<List<CrimeBotItem>>(arguments[0].ToString())
            //            .Where(item => item.Count > 0).ToList();

            //    decimal sum = 0;
            //    foreach (CrimeBotItem item in items)
            //    {
            //        if (item.Count != 0) sum += item.Cost * item.Count;
            //    }

            //    CharacterEntity character = sender.GetAccountEntity().CharacterEntity;

            //    if (!character.HasMoney(sum))
            //    {
            //        SendMessageToNerbyPlayers(
            //            $"Co to jest? Brakuje ${sum - sender.GetAccountEntity().CharacterEntity.DbModel.Money}, forsa w gotówce", ChatMessageType.Normal);
            //        return;
            //    }

            //    //Sprawdzamy czy gracz nie chce kupić więcej niż ma bot
            //    if (items.Any(crimeBotItem => Items.First(x => x.Name == crimeBotItem.Name).Count < crimeBotItem.Count))
            //    {
            //        return;
            //    }

            //    using (ItemsRepository repository = new ItemsRepository())
            //    {
            //        foreach (CrimeBotItem i in items)
            //        {
            //            //TYPY: 0 broń, 1 amunicja, 2 narkotyki 
            //            ItemModel item = new ItemModel();

            //            if (i.Type == ItemEntityType.Weapon)
            //            {
            //                Tuple<WeaponHash, int?> data = Constant.Items.GetWeaponData(i.Name);

            //                item.Character = sender.GetAccountEntity().CharacterEntity.DbModel;
            //                item.CreatorId = null;
            //                item.Name = i.Name;
            //                item.ItemEntityType = i.Type;
            //                item.FirstParameter = (int)data.Item1;
            //                item.SecondParameter = data.Item2;
            //            }
            //            else if (i.Type == ItemEntityType.WeaponClip)
            //            {
            //                Tuple<WeaponHash, int?> data = Constant.Items.GetWeaponData(i.Name);

            //                item.Character = sender.GetAccountEntity().CharacterEntity.DbModel;
            //                item.CreatorId = null;
            //                item.Name = i.Name;
            //                item.ItemEntityType = i.Type;
            //                item.FirstParameter = (int)data.Item1;
            //                item.SecondParameter = data.Item2;
            //            }
            //            else if (i.Type == ItemEntityType.Drug)
            //            {
            //                item.Character = sender.GetAccountEntity().CharacterEntity.DbModel;
            //                item.CreatorId = null;
            //                item.Name = i.Name;
            //                item.ItemEntityType = i.Type;
            //                item.FirstParameter = (int)Enum.Parse(typeof(DrugType), i.Name);
            //            }
            //            else
            //            {
            //                return;
            //            }

            //            PropertyInfo field = typeof(CrimeBotModel).GetProperties().Single(f => f.Name == i.DatabaseField);
            //            field.SetValue(DbModel, (int)field.GetValue(DbModel) - i.Count);

            //            repository.Insert(item);
            //        }
            //        repository.Save();
            //    }
            //    character.RemoveMoney(sum);


            //    using (CrimeBotsRepository repository = new CrimeBotsRepository())
            //    {
            //        repository.Update(DbModel);
            //        repository.Save();
            //    }

            //    EndTransaction();
            //    Dispose(false);
            //}
        }

        private void BotShape_OnEntityEnterColShape(ColShape shape, Client entity)
        {
            if (entity == Player.Client)
            {
                NAPI.ClientEvent.TriggerClientEvent(Player.Client, "ShowCrimeBotCef", JsonConvert.SerializeObject(Items.OrderBy(x => x.Type)));
            }
            else
                SendMessageToNerbyPlayers("Odwal się", ChatMessageType.Normal);

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void Dispose(bool disposing)
        {
            BotShape.OnEntityEnterColShape -= BotShape_OnEntityEnterColShape;
            NAPI.ClientEvent.TriggerClientEvent(Player.Client, "DisposeCrimeBotComponents");
            if (disposing)
            {
                Dispose();
            }
        }

        private void EndTransaction()
        {
            SendMessageToNerbyPlayers("Interesy z Tobą to przyjemność", ChatMessageType.Normal);
            Vehicle.GameVehicle.CloseDoor(5);
        }
    }
}