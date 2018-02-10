﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Core.Serialization.Xml;
using Serverside.CrimeBot.Models;
using Serverside.Entities.Core;
using Serverside.Groups.Base;

namespace Serverside.CrimeBot
{
    public class CrimeBotScript : Script
    {
        [Command("dodajbotp")]
        public void AddCrimeBot(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do tworzenia bota przestępczego.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            FullPosition botPosition = null;
            VehicleEntity botVehicle = null;

            Event.OnChatMessage += Handler;

            void Handler(Client o, string message, CancelEventArgs cancel)
            {
                if (o == sender && message == "tu" && botPosition == null)
                {
                    cancel.Cancel = true;
                    botPosition = new FullPosition
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
                        },

                        Direction = new Vector3(0f, 0f, 0f)
                    };

                    NAPI.ClientEvent.TriggerClientEvent(sender, "DrawAddingCrimeBotComponents", new Vector3(botPosition.Position.X, botPosition.Position.Y, botPosition.Position.Z - 1));
                    sender.Notify("Ustaw pojazd w wybranej pozycji następnie wpisz \"tu\".");

                    botVehicle = VehicleEntity.Create(Event, new FullPosition(new Vector3(sender.Position.X + 2, sender.Position.Y + 2, sender.Position.Z), sender.Rotation), VehicleHash.Sentinel, name, 0, sender.GetAccountEntity().DbModel, new Color(0, 0, 0), new Color(0, 0, 0));

                    NAPI.Player.SetPlayerIntoVehicle(sender, botVehicle.GameVehicle, -1);

                }
                else if (o == sender && message == "tu" && botPosition != null && botVehicle != null)
                {
                    cancel.Cancel = true;

                    var botVehiclePosition = new FullPosition
                    {
                        Position = new Vector3
                        {
                            X = botVehicle.GameVehicle.Position.X,
                            Y = botVehicle.GameVehicle.Position.Y,
                            Z = botVehicle.GameVehicle.Position.Z
                        },

                        Rotation = new Vector3
                        {
                            X = botVehicle.GameVehicle.Rotation.X,
                            Y = botVehicle.GameVehicle.Rotation.Y,
                            Z = botVehicle.GameVehicle.Rotation.Z
                        },

                        Direction = new Vector3(0f, 0f, 0f)
                    };

                    NAPI.ClientEvent.TriggerClientEvent(sender, "DisposeAddingCrimeBotComponents");

                    XmlHelper.AddXmlObject(new CrimeBotPosition
                    {
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                        Name = name,
                        BotPosition = botPosition,
                        VehiclePosition = botVehiclePosition
                    }, $@"{Constant.ServerInfo.XmlDirectory}CrimeBotPositions\");

                    sender.Notify("Dodawanie pozycji bota zakończyło się ~h~~g~pomyślnie!");
                    NAPI.Player.WarpPlayerOutOfVehicle(sender);
                    botVehicle.Dispose();
                    Event.OnChatMessage -= Handler;
                }
            };
        }

        [Command("usunbotp", "~y~ UŻYJ ~w~ /usunbotp (nazwa)", GreedyArg = true)]
        public void DeleteCrimeBotPosition(Client sender, string name = "")
        {
            CrimeBotPosition position = null;
            var positions = XmlHelper
                .GetXmlObjects<CrimeBotPosition>($@"{Constant.ServerInfo.XmlDirectory}CrimeBotPositions\");

            if (name != "")
            {
                position = positions.OrderBy(a => a.BotPosition.Position.DistanceTo(sender.Position)).First();
            }
            else
            {
                if (positions.Any(f => f.Name == name))
                {
                    position = positions.First(x => x.Name == name);
                }
            }

            if (position != null && XmlHelper.TryDeleteXmlObject(position.FilePath))
            {
                sender.Notify("Usuwanie pozycji bota zakończyło się ~h~~g~pomyślnie.");
            }
            else
            {
                sender.Notify("Usuwanie pozycji bota zakończyło się ~h~~r~niepomyślnie.");
            }
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            //args[0] to index na liście pozycji
            if (eventName == "OnPlayerSelectedCrimeBotDiscrict")
            {
                var player = sender.GetAccountEntity();
                if (player.CharacterEntity.OnDutyGroup is CrimeGroup group)
                {
                    if (group.CrimeBot != null)
                    {
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                        return;
                    }

                    using (CrimeBotsRepository repository = new CrimeBotsRepository())
                    {
                        var crimeBotData = repository.Get(group.DbModel);
                        var position = XmlHelper.GetXmlObjects<CrimeBotPosition>($@"{Constant.ServerInfo.XmlDirectory}CrimeBotPositions\")[Convert.ToInt32(arguments[0])];

                        group.CrimeBot = new CrimeBot(player, group, position.VehiclePosition, Event, crimeBotData.Name, crimeBotData.Model, position.BotPosition);
                        group.CrimeBot.Intialize();
                        sender.TriggerEvent("DrawCrimeBotComponents", position.BotPosition.Position, 500, 2);
                    }
                }
                else
                {
                    sender.Notify("Aby wezwać sprzedawcę musisz znajdować się na służbie grupy.");
                }
            }
        }
    }
}