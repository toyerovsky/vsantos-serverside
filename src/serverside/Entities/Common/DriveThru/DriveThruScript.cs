/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Constant;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Core.Serialization;
using Serverside.Entities.Common.DriveThru.Models;
using Serverside.Entities.Core;
using Serverside.Entities.Core.Item;

namespace Serverside.Entities.Common.DriveThru
{
    public class DriveThruScript : Script
    {
        private List<DriveThruEntity> DriveThrus { get; set; } = new List<DriveThruEntity>();

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "OnPlayerDriveThruBought")
            {
                decimal money = Convert.ToDecimal(arguments[2]);
                if (!sender.HasMoney(money))
                {
                    sender.Notify("Nie posiadasz wystarczającej ilości gotówki.");
                    return;
                }
                sender.RemoveMoney(money);

                AccountEntity player = sender.GetAccountEntity();

                ItemModel itemModel = new ItemModel
                {
                    Name = (string)arguments[0],
                    Character = player.CharacterEntity.DbModel,
                    Creator = null,
                    ItemType = ItemType.Food,
                    FirstParameter = (int)arguments[1],
                };

                using (ItemsRepository repository = new ItemsRepository())
                {
                    repository.Insert(itemModel);
                    repository.Save();
                }
                sender.Notify($"Pomyślnie zakupiłeś ~h~{itemModel.Name} za ${money}.");
            }
        }

        [ServerEvent(Event.ResourceStart)]
        private void OnResourceStart()
        {
            foreach (DriveThruModel data in XmlHelper.GetXmlObjects<DriveThruModel>(Path.Combine(ServerInfo.XmlDirectory, "DriveThrus")))
            {
                DriveThruEntity driveThru = new DriveThruEntity(data);
                driveThru.Spawn();
                DriveThrus.Add(driveThru);
            }
        }

        [Command("dodajdrivethru", GreedyArg = true)]
        public void AddDriveThru(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do dodawania DriveThru.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            Vector3 center = null;

            void Handler(Client o, string message)
            {
                if (center == null && o == sender && message == "tu")
                {
                    center = o.Position;
                    DriveThruModel data = new DriveThruModel
                    {
                        Position = o.Position,
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                    };
                    XmlHelper.AddXmlObject(data, $"{ServerInfo.XmlDirectory}DriveThrus\\");

                    sender.Notify("Dodawanie DriveThru zakończyło się ~g~~h~pomyślnie.");
                    DriveThruEntity driveThru = new DriveThruEntity(data);
                    driveThru.Spawn();
                    DriveThrus.Add(driveThru);                    
                }
            }
        }

        [Command("usundrivethru")]
        public void DeleteDriveThru(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania przystanku DriveThru.");
                return;
            }

            if (DriveThrus.Count == 0)
            {
                sender.Notify("Nie znaleziono DriveThru które można usunąć.");
                return;
            }
            DriveThruEntity driveThru = DriveThrus.OrderBy(d => d.Data.Position.DistanceTo2D(sender.Position)).First();
            if (XmlHelper.TryDeleteXmlObject(driveThru.Data.FilePath))
            {
                sender.Notify("Usuwanie DriveThru zakończyło się ~g~~h~pomyślnie.");
                DriveThrus.Remove(driveThru);
                driveThru.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie DriveThru zakończyło się ~r~~h~niepomyślnie.");
            }
        }
    }
}