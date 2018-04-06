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
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Core.Telephone;
using VRP.Serverside.Entities.Common.Booth.Models;
using VRP.Serverside.Entities.Core;
using ChatMessageType = VRP.Core.Enums.ChatMessageType;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Common.Booth
{
    public class TelephoneBoothScript : Script
    {
        private List<TelephoneBoothEntity> Booths { get; set; } = new List<TelephoneBoothEntity>();

        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            //args[0] to numer na jaki dzwoni
            if (eventName == "OnPlayerTelephoneBoothCall")
            {
                CharacterEntity senderCharacter = sender.GetAccountEntity().CharacterEntity;
                if (senderCharacter.CurrentInteractive is TelephoneBoothEntity boothEntity)
                {
                    if (senderCharacter.HasMoney(boothEntity.Data.Cost))
                    {
                        senderCharacter.RemoveMoney(boothEntity.Data.Cost);
                        ChatScript.SendMessageToNearbyPlayers(senderCharacter, "wrzuca monetę do automatu i wybiera numer", ChatMessageType.ServerMe);

                        CharacterEntity getterCharacter = EntityHelper.GetAccounts()
                            .FirstOrDefault(t => t.CharacterEntity?.CurrentCellphone.Number ==
                                      Convert.ToInt32(args[0]))
                            ?.CharacterEntity;

                        if (getterCharacter == null)
                        {
                            sender.SendChatMessage("~#ffdb00~",
                                "Wybrany abonent ma wyłączony telefon bądź znajduje się poza zasięgiem spróbuj później.");
                            return;
                        }

                        if (getterCharacter.CurrentCellphone.CurrentCall != null)
                        {
                            sender.SendChatMessage("~#ffdb00~",
                                "Wybrany abonent prowadzi obecnie rozmowę. Spróbuj później.");
                            return;
                        }

                        boothEntity.CurrentCall = new TelephoneCall(senderCharacter, getterCharacter, boothEntity.Data.Number);

                        boothEntity.CurrentCall.Timer.Elapsed += (o, eventArgs) =>
                        {
                            sender.SendChatMessage("~#ffdb00~",
                                "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                            boothEntity.CurrentCall.Dispose();
                            boothEntity.CurrentCall = null;
                        };
                    }

                }
                else
                {
                    sender.Notify("Nie posiadasz wystarczającej ilości gotówki.", NotificationType.Error);
                }

            }
            else if (eventName == "OnPlayerTelephoneBoothEnd" && sender.HasData("Booth"))
            {
                CharacterEntity senderCharacter = sender.GetAccountEntity().CharacterEntity;

                if (senderCharacter.CurrentInteractive is TelephoneBoothEntity boothEntity)
                {
                    if (boothEntity.CurrentCall != null && boothEntity.CurrentCall.CurrentlyTalking)
                    {
                        boothEntity.CurrentCall.Sender.Notify(
                            "Rozmowa zakończona.", NotificationType.Info);
                        boothEntity.CurrentCall.Getter.Notify(
                            "Rozmowa zakończona.", NotificationType.Info);

                        boothEntity.CurrentCall.Dispose();
                        boothEntity.CurrentCall = null;
                    }
                }
            }
        }

        #region ADMIN COMMANDS
        [Command("dodajbudke")]
        public void CreateAtm(Client sender, decimal cost, string number)
        {
            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu.\" użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.", NotificationType.Info);

            if (!ValidationHelper.IsMoneyValid(cost) || !ValidationHelper.IsCellphoneNumberValid(number))
            {
                sender.Notify("Wprowadzono dane w nieprawidłowym formacie.", NotificationType.Error);
                return;
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

                    XmlHelper.AddXmlObject(data, Path.Combine(Utils.XmlDirectory, nameof(TelephoneBoothModel)));
                    TelephoneBoothEntity booth = new TelephoneBoothEntity(data);
                    booth.Spawn();
                    Booths.Add(booth);
                    sender.Notify("Dodawanie budki zakończyło się pomyślnie.", NotificationType.Info);
                }
            }
        }

        [Command("usunbudke")]
        public void DeleteBusStop(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do usuwania budki.", NotificationType.Error);
                return;
            }

            if (Booths.Count == 0)
            {
                sender.Notify("Nie znaleziono budki telefonicznej którą można usunąć.", NotificationType.Warning);
                return;
            }

            TelephoneBoothEntity telephoneBooth = Booths.OrderBy(a => a.Data.Position.Position.DistanceTo(sender.Position)).First();
            if (XmlHelper.TryDeleteXmlObject(telephoneBooth.Data.FilePath))
            {
                sender.Notify("Usuwanie budki telefonicznej zakończyło się pomyślnie.", NotificationType.Info);
                Booths.Remove(telephoneBooth);
                telephoneBooth.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie budki telefonicznej zakończyło się niepomyślnie.", NotificationType.Error);
            }
        }
        #endregion
    }
}
