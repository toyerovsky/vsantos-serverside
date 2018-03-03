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
using Serverside.Core;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Core.Serialization;
using Serverside.Core.Telephone;
using Serverside.Entities.Common.Booth.Models;

namespace Serverside.Entities.Common.Booth
{
    public class TelephoneBoothScript : Script
    {
        private List<TelephoneBoothEntity> Booths { get; set; } = new List<TelephoneBoothEntity>();

        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            //args[0] to numer na jaki dzwoni
            if (eventName == "OnPlayerTelephoneBoothCall")
            {
                var characterEntity = sender.GetAccountEntity().CharacterEntity;
                if (characterEntity.CurrentInteractive is TelephoneBoothEntity boothEntity)
                {
                    if (sender.HasMoney(boothEntity.Data.Cost))
                    {
                        sender.RemoveMoney(boothEntity.Data.Cost);
                        ChatScript.SendMessageToNearbyPlayers(sender, "wrzuca monetę do automatu i wybiera numer", ChatMessageType.ServerMe);

                        if (EntityHelper.GetAccounts().Values.Any(t => t.CharacterEntity.CurrentCellphone.Number == Convert.ToInt32(args[0])))
                        {
                            Client getterPlayer = EntityHelper.GetAccounts().Values
                                .Single(t => t.CharacterEntity.CurrentCellphone.Number ==
                                          Convert.ToInt32(args[0])).Client;

                            if (getterPlayer.HasData("CellphoneTalking"))
                            {
                                sender.SendChatMessage("~#ffdb00~",
                                    "Wybrany abonent prowadzi obecnie rozmowę, spróbuj później.");
                                return;
                            }

                            boothEntity.CurrentCall = new TelephoneCall(sender, getterPlayer, boothEntity.Data.Number);

                            boothEntity.CurrentCall.Timer.Elapsed += (o, eventArgs) =>
                            {
                                sender.SendChatMessage("~#ffdb00~",
                                    "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                                boothEntity.CurrentCall.Dispose();
                                boothEntity.CurrentCall = null;
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
            }
            else if (eventName == "OnPlayerTelephoneBoothEnd" && sender.HasData("Booth"))
            {
                TelephoneBoothEntity boothEntity = sender.GetData("Booth");

                if (boothEntity.CurrentCall != null && boothEntity.CurrentCall.CurrentlyTalking)
                {
                    boothEntity.CurrentCall.Sender.SendChatMessage("~#ffdb00~",
                        "Rozmowa zakończona.");
                    boothEntity.CurrentCall.Getter.SendChatMessage("~#ffdb00~",
                        "Rozmowa zakończona.");

                    boothEntity.CurrentCall.Dispose();
                    boothEntity.CurrentCall = null;
                }
            }
        }

        #region ADMIN COMMANDS
        [Command("dodajbudke")]
        public void CreateAtm(Client sender, decimal cost, string number)
        {
            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu.\"");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            if (!ValidationHelper.IsMoneyValid(cost) || !ValidationHelper.IsCellphoneNumberValid(number))
            {
                sender.Notify("Wprowadzono dane w nieprawidłowym formacie.");
            }

            void Handler(Client o, string message)
            {
                if (o == sender && message == "/tu")
                {
                    TelephoneBoothModel data = new TelephoneBoothModel
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

                    XmlHelper.AddXmlObject(data, Path.Combine(Constant.ServerInfo.XmlDirectory, nameof(TelephoneBoothModel)));
                    var booth = new TelephoneBoothEntity(data);
                    booth.Spawn();
                    Booths.Add(booth);
                    sender.Notify("Dodawanie budki zakończyło się ~g~~h~pomyślnie.");
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
