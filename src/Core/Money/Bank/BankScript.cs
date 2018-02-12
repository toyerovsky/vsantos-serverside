/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Core.Serialization.Xml;
using Serverside.Entities.Common.Atm;
using Serverside.Entities.Common.Atm.Models;

namespace Serverside.Core.Money.Bank
{
    public class BankScript : Script
    {
        private List<AtmEntity> Atms { get; set; } = new List<AtmEntity>();

        public BankScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            foreach (var atm in XmlHelper.GetXmlObjects<AtmModel>($@"{Constant.ServerInfo.XmlDirectory}Atms\"))
            {
                Atms.Add(new AtmEntity(Event, atm));
            }
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "OnPlayerAtmTake")
            {
                if (decimal.TryParse(arguments[0].ToString(), out decimal money))
                {
                    ChatScript.SendMessageToNearbyPlayers(sender,
                        $"wkłada {(money >= 3000 ? "gruby" : "chudy")} plik gotówki do bankomatu i po przetworzeniu operacji zabiera kartę.", ChatMessageType.ServerMe);
                    BankHelper.DepositMoney(sender, money);
                }
            }
            else if (eventName == "OnPlayerAtmGive")
            {
                if (decimal.TryParse(arguments[0].ToString(), out decimal money))
                {
                    ChatScript.SendMessageToNearbyPlayers(sender,
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
                sender.Notify("Nie posiadasz uprawnień do tworzenia grupy.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");
            
            Event.OnChatMessage += Handler;

            void Handler(Client o, string message, CancelEventArgs cancel)
            {
                if (o == sender && message == "tu")
                {
                    AtmModel atm = new AtmModel
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
                    XmlHelper.AddXmlObject(atm, $@"{Constant.ServerInfo.XmlDirectory}Atms\");
                    Atms.Add(new AtmEntity(Event, atm)); //Nowa instancja bankomatu spawnuje go w świecie gry
                    sender.Notify("Dodawanie bankomatu zakończyło się ~h~~g~pomyślnie.");
                    Event.OnChatMessage -= Handler;
                }
            }
        }

        [Command("usunbankomat")]
        public void DeleteAtm(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania bankomatu.");
                return;
            }

            if (Atms.Count == 0)
            {
                sender.Notify("Nie znaleziono bankomatu który można usunąć.");
                return;
            }

            var atm = Atms.First(a => a.ColShape.IsPointWithin(sender.Position));
            if (XmlHelper.TryDeleteXmlObject(atm.Data.FilePath))
            {
                sender.Notify("Usuwanie bankomatu zakończyło się ~h~~g~pomyślnie.");
                Atms.Remove(atm);
                atm.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie bankomatu zakończyło się ~h~~r~niepomyślnie.");
            }
        }
        #endregion

    }
}
