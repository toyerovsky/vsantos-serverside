/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Common.DriveThru.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Common.DriveThru
{
    public class DriveThruScript : Script
    {
        private List<DriveThruEntity> DriveThrus { get; set; } = new List<DriveThruEntity>();

        [RemoteEvent(RemoteEvents.OnPlayerDriveThruBought)]
        public void OnPlayerDriveThruBoughtHandler(Client sender, params object[] arguments)
        {
            decimal money = Convert.ToDecimal(arguments[2]);
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (!character.HasMoney(money))
            {
                sender.SendInfo("Nie posiadasz wystarczającej ilości gotówki.");
                return;
            }
            character.RemoveMoney(money);

            AccountEntity player = sender.GetAccountEntity();

            ItemModel itemModel = new ItemModel
            {
                Name = (string)arguments[0],
                Character = player.CharacterEntity.DbModel,
                Creator = null,
                ItemEntityType = ItemEntityType.Food,
                FirstParameter = (int)arguments[1],
            };

            using (ItemsRepository repository = new ItemsRepository())
            {
                repository.Insert(itemModel);
                repository.Save();
            }
            sender.SendInfo($"Pomyślnie zakupiłeś {itemModel.Name} za ${money}.");
        }

 

        [ServerEvent(Event.ResourceStart)]
        private void OnResourceStart()
        {
            foreach (DriveThruModel data in XmlHelper.GetXmlObjects<DriveThruModel>(Path.Combine(Utils.XmlDirectory, "DriveThrus")))
            {
                DriveThruEntity driveThru = new DriveThruEntity(data);
                driveThru.Spawn();
                DriveThrus.Add(driveThru);
            }
        }

        [Command("dodajdrivethru", GreedyArg = true)]
        public void AddDriveThru(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.AdministratorGry)
            {
                sender.SendWarning("Nie posiadasz uprawnień do dodawania DriveThru.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\" ctrl + alt + shift + d użyj /diag aby poznać swoją obecną pozycję.");
            
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
                    XmlHelper.AddXmlObject(data, Path.Combine(Utils.XmlDirectory, "DriveThrus"));

                    sender.SendInfo("Dodawanie DriveThru zakończyło się pomyślnie.");
                    DriveThruEntity driveThru = new DriveThruEntity(data);
                    driveThru.Spawn();
                    DriveThrus.Add(driveThru);                    
                }
            }
        }

        [Command("usundrivethru")]
        public void DeleteDriveThru(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.AdministratorGry)
            {
                sender.SendWarning("Nie posiadasz uprawnień do usuwania przystanku DriveThru.");
                return;
            }

            if (DriveThrus.Count == 0)
            {
                sender.SendWarning("Nie znaleziono DriveThru które można usunąć.");
                return;
            }
            DriveThruEntity driveThru = DriveThrus.OrderBy(d => d.Data.Position.DistanceTo2D(sender.Position)).First();
            if (XmlHelper.TryDeleteXmlObject(driveThru.Data.FilePath))
            {
                sender.SendInfo("Usuwanie DriveThru zakończyło się pomyślnie.");
                DriveThrus.Remove(driveThru);
                driveThru.Dispose();
            }
            else
            {
                sender.SendError("Usuwanie DriveThru zakończyło się niepomyślnie.");
            }
        }
    }
}