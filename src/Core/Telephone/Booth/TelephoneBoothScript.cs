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
using Serverside.Core.Enums;
using Serverside.Core.Telephone.Booth.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Core.Serialization.Xml;
using Serverside.Entities;

namespace Serverside.Core.Telephone.Booth
{
    public class TelephoneBoothScript : Script
    {
        private List<TelephoneBooth> Booths { get; set; } = new List<TelephoneBooth>();

        public TelephoneBoothScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            foreach (var booth in XmlHelper.GetXmlObjects<TelephoneBoothModel>(Path.Combine(Constant.ServerInfo.XmlDirectory, "Booths")))
            {
                //W konstruktorze spawnujemy budkę telefoniczną do gry
                Booths.Add(new TelephoneBooth(Event, booth));
            }
        }

        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            //args[0] to numer na jaki dzwoni
            if (eventName == "OnPlayerTelephoneBoothCall" && sender.HasData("Booth"))
            {
                TelephoneBooth booth = sender.GetData("Booth");
                if (sender.HasMoney(booth.Data.Cost))
                {
                    sender.RemoveMoney(booth.Data.Cost);
                    ChatScript.SendMessageToNearbyPlayers(sender, "wrzuca monetę do automatu i wybiera numer", ChatMessageType.ServerMe);
                    
                    if (EntityManager.GetAccounts().Values.Any(t => t.CharacterEntity.CurrentCellphone.Number == Convert.ToInt32(args[0])))
                    {
                        Client getterPlayer = EntityManager.GetAccounts().Values
                            .Single(t => t.CharacterEntity.CurrentCellphone.Number ==
                                      Convert.ToInt32(args[0])).Client;

                        if (getterPlayer.HasData("CellphoneTalking"))
                        {
                            sender.SendChatMessage("~#ffdb00~",
                                "Wybrany abonent prowadzi obecnie rozmowę, spróbuj później.");
                            return;
                        }

                        booth.CurrentCall = new TelephoneCall(sender, getterPlayer, booth.Data.Number);

                        booth.CurrentCall.Timer.Elapsed += (o, eventArgs) =>
                        {
                            sender.SendChatMessage("~#ffdb00~",
                                "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                            booth.CurrentCall.Dispose();
                            booth.CurrentCall = null;
                        };
                    }
                    else
                    {
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                    }
                }
                else
                {
                    sender.Notify("Nie posiadasz wystarczającej ilości gotówki.");
                }
            }
            else if (eventName == "OnPlayerTelephoneBoothEnd" && sender.HasData("Booth"))
            {
                TelephoneBooth booth = sender.GetData("Booth");

                if (booth.CurrentCall != null && booth.CurrentCall.CurrentlyTalking)
                {
                    booth.CurrentCall.Sender.SendChatMessage("~#ffdb00~",
                        "Rozmowa zakończona.");
                    booth.CurrentCall.Getter.SendChatMessage("~#ffdb00~",
                        "Rozmowa zakończona.");

                    booth.CurrentCall.Dispose();
                    booth.CurrentCall = null;
                }
            }
        }

        #region ADMIN COMMANDS
        [Command("dodajbudke")]
        public void CreateAtm(Client sender, decimal cost, string number)
        {
            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu.\"");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            if (!Validator.IsMoneyValid(cost) || !Validator.IsCellphoneNumberValid(number))
            {
                sender.Notify("Wprowadzono dane w nieprawidłowym formacie.");
            }

            Event.OnChatMessage += Handler;

            void Handler(Client o, string message, CancelEventArgs cancel)
            {
                if (o == sender && message == "/tu")
                {
                    TelephoneBoothModel booth = new TelephoneBoothModel
                    {
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                        Position = new FullPosition
                        {
                            Position = new Vector3
                            {
                                X = sender.Position.X,
                                Y = sender.Position.Y,
                                Z = sender.Position.Z
                            },

                            Rotation = new Vector3
                            {
                                X = sender.Rotation.X,
                                Y = sender.Rotation.Y,
                                Z = sender.Rotation.Z
                            }
                        },
                        Cost = cost,
                        Number = int.Parse(number)
                    };

                    XmlHelper.AddXmlObject(booth, Constant.ServerInfo.XmlDirectory + @"Booths\");
                    Booths.Add(new TelephoneBooth(Event, booth));
                    sender.Notify("Dodawanie budki zakończyło się ~g~~h~pomyślnie.");
                    Event.OnChatMessage -= Handler;
                }
            }
        }

        [Command("usunbudke")]
        public void DeleteBusStop(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania budki.");
                return;
            }

            if (Booths.Count == 0)
            {
                sender.Notify("Nie znaleziono budki telefonicznej którą można usunąć.");
                return;
            }

            var telephoneBooth = Booths.OrderBy(a => a.Data.Position.Position.DistanceTo(sender.Position)).First();
            if (XmlHelper.TryDeleteXmlObject(telephoneBooth.Data.FilePath))
            {
                sender.Notify("Usuwanie budki telefonicznej zakończyło się ~g~~h~pomyślnie.");
                Booths.Remove(telephoneBooth);
                telephoneBooth.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie budki telefonicznej zakończyło ~r~~h~się niepomyślnie.");
            }
        }
        #endregion
    }
}
