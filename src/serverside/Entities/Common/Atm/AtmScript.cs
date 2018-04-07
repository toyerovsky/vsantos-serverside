/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Economy.Bank;
using VRP.Serverside.Entities.Common.Atm.Models;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Common.Atm
{
    public class AtmScript : Script
    {
        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "OnPlayerAtmTake")
            {
                if (decimal.TryParse(arguments[0].ToString(), out decimal money))
                {
                    ChatScript.SendMessageToNearbyPlayers(sender.GetAccountEntity().CharacterEntity,
                        $"wkłada {(money >= 3000 ? "gruby" : "chudy")} plik gotówki do bankomatu i po przetworzeniu operacji zabiera kartę.", ChatMessageType.ServerMe);
                    BankHelper.DepositMoney(sender, money);
                }
            }
            else if (eventName == "OnPlayerAtmGive")
            {
                if (decimal.TryParse(arguments[0].ToString(), out decimal money))
                {
                    ChatScript.SendMessageToNearbyPlayers(sender.GetAccountEntity().CharacterEntity,
                        $"wyciąga z bankomatu {(money >= 3000 ? "gruby" : "chudy")} plik gotówki, oraz kartę.", ChatMessageType.ServerMe);
                    BankHelper.WithdrawMoney(sender, money);
                }
            }
        }

        #region ADMIN COMMANDS

        [Command("dodajbankomat")]
        public void CreateAtm(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendWarning("Nie posiadasz uprawnień do dodawania bankomatu.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\". użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");

            void Handler(Client o, string message)
            {
                if (o == sender && message == "tu")
                {
                    AtmModel data = new AtmModel
                    {
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                        Position = new FullPosition
                        {
                            Position = new Vector3
                            {
                                X = o.Position.X,
                                Y = o.Position.Y,
                                Z = o.Position.Z
                            },

                            Rotation = new Vector3
                            {
                                X = o.Rotation.X,
                                Y = o.Rotation.Y,
                                Z = o.Rotation.Z
                            }
                        }
                    };
                    XmlHelper.AddXmlObject(data, Path.Combine(Utils.XmlDirectory, nameof(AtmModel)));
                    AtmEntity atm = new AtmEntity(data);
                    atm.Spawn();
                    EntityHelper.Add(atm);
                    sender.SendInfo("Dodawanie bankomatu zakończyło się pomyślnie.");

                }
            }
        }

        [Command("usunbankomat")]
        public void DeleteAtm(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendWarning("Nie posiadasz uprawnień do usuwania bankomatu.");
                return;
            }

            if (!EntityHelper.GetAtms().Any())
            {
                sender.SendError("Nie znaleziono bankomatu który można usunąć.");
                return;
            }

            AtmEntity atm = EntityHelper.GetAtms().First(a => a.ColShape.IsPointWithin(sender.Position));
            if (XmlHelper.TryDeleteXmlObject(atm.Data.FilePath))
            {
                sender.SendInfo("Usuwanie bankomatu zakończyło się pomyślnie.");
                EntityHelper.Remove(atm);
                atm.Dispose();
            }
            else
            {
                sender.SendError("Usuwanie bankomatu zakończyło się niepomyślnie.");
            }
        }
        #endregion

    }
}
