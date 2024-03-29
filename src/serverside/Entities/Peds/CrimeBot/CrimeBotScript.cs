﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Enums;
using VRP.DAL.Repositories;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Vehicle;
using VRP.Serverside.Entities.Peds.CrimeBot.Models;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Peds.CrimeBot
{
    public class CrimeBotScript : Script
    {
        [Command("dodajbotp")]
        public void AddCrimeBot(Client sender, string name)
        {
            if (!sender.HasRank(ServerRank.AdministratorRozgrywki3))
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia bota przestępczego.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\" użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");

            FullPosition botPosition = null;
            VehicleEntity botVehicle = null;

            void Handler(Client o, string message)
            {
                if (o == sender && message == "tu" && botPosition == null)
                {
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

                    NAPI.ClientEvent.TriggerClientEvent(sender,
                        "DrawAddingCrimeBotComponents",
                        new Vector3(botPosition.Position.X, botPosition.Position.Y, botPosition.Position.Z - 1));

                    sender.SendInfo("Ustaw pojazd w wybranej pozycji następnie wpisz \"tu\".");

                    botVehicle = VehicleEntity.Create(
                        new FullPosition(new Vector3(
                                sender.Position.X + 2,
                                sender.Position.Y + 2,
                                sender.Position.Z), sender.Rotation),
                        VehicleHash.Sentinel, name, 0, sender.GetAccountEntity().DbModel.Id, new Color(0, 0, 0), new Color(0, 0, 0));

                    NAPI.Player.SetPlayerIntoVehicle(sender, botVehicle.GameVehicle, -1);

                }
                else if (o == sender && message == "tu" && botPosition != null && botVehicle != null)
                {
                    FullPosition botVehiclePosition = new FullPosition
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
                    }, Path.Combine(Utils.XmlDirectory, "CrimeBotPositions"));

                    sender.SendInfo("Dodawanie pozycji bota zakończyło się pomyślnie!");
                    NAPI.Player.WarpPlayerOutOfVehicle(sender);
                    botVehicle.Dispose();
                }
            };
        }

        [Command("usunbotp", "~y~ UŻYJ ~w~ /usunbotp (nazwa)", GreedyArg = true)]
        public void DeleteCrimeBotPosition(Client sender, string name = "")
        {
            CrimeBotPosition position = null;
            List<CrimeBotPosition> positions = XmlHelper
                .GetXmlObjects<CrimeBotPosition>(Path.Combine(Utils.XmlDirectory, "CrimeBotPositions"));

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
                sender.SendInfo("Usuwanie pozycji bota zakończyło się pomyślnie.");
            }
            else
            {
                sender.SendError("Usuwanie pozycji bota zakończyło się niepomyślnie.");
            }
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            //args[0] to index na liście pozycji
            if (eventName == "OnPlayerSelectedCrimeBotDiscrict")
            {
                AccountEntity player = sender.GetAccountEntity();
                if (player.CharacterEntity.OnDutyGroup is CrimeGroup group && group.CanPlayerCallCrimeBot(player))
                {
                    if (group.CrimePedEntity != null)
                    {
                        // crime bot already spawned
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                        return;
                    }

                    RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
                    using (CrimeBotsRepository repository = new CrimeBotsRepository(ctx))
                    {
                        CrimeBotModel crimeBotData = repository.Get(crimeBot => crimeBot.GroupModel.Id == group.Id);
                        CrimeBotPosition position = XmlHelper.GetXmlObjects<CrimeBotPosition>(
                            Path.Combine(Utils.XmlDirectory, "CrimeBotPositions"))[Convert.ToInt32(arguments[0])];

                        group.CrimePedEntity = new CrimePedEntity(player, group, position.VehiclePosition,
                            crimeBotData.Name, NAPI.Util.PedNameToModel(crimeBotData.PedSkin), position.BotPosition);
                        group.CrimePedEntity.Spawn();
                        sender.TriggerEvent("DrawCrimeBotComponents", position.BotPosition.Position, 500, 2);
                    }
                }
                else
                {
                    sender.SendChatMessage("~#ffdb00~",
                        "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                }
            }
        }
    }
}