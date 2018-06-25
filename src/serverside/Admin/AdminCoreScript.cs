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
using VRP.Core.Tools;
using VRP.DAL.Enums;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using Color = System.Drawing.Color;

namespace VRP.Serverside.Admin
{
    public class AdminCoreScript : Script
    {
        [Command("ustawrange", "~y~ UŻYJ ~w~ /ustawrange [id] [nazwa]")]
        public void SetRank(Client sender, int id, ServerRank rank)
        {
            if (!sender.HasRank(ServerRank.AdministratorTechniczny3))
            {
                sender.SendWarning("Nie posiadasz uprawnień do ustawiania rang.");
                return;
            }

            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }

            controller.DbModel.ServerRank = rank;
            controller.Save();
            sender.SendInfo($"Nadałeś {controller.CharacterEntity.FormatName} ({controller.DbModel.Name}) rangę {controller.DbModel.ServerRank}");
        }

        [Command("tpc", "~y~ UŻYJ ~w~ /tpc [x] [y] [z]")]
        public void TeleportToCords(Client sender, float x, float y, float z)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do teleportu na koordynaty.");
                return;
            }
            sender.Position = new Vector3(x, y, z);
            sender.SendInfo($"Teleportowałeś się na X:{x} Y:{y} Z:{z}");
        }

        [Command("tpmap", "~y~ UŻYJ ~w~ /tpmap")]
        public void TeleportToWaypoint(Client sender)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do teleportu na waypoint.");
                return;
            }
            Action<Vector3> teleportAction = position => sender.Position = position;

            sender.SetData("WaypointVectorHandler", teleportAction);
            sender.TriggerEvent("GetWaypointPosition");

        }

        [Command("tp", "~y~ UŻYJ ~w~ /tp [id]")]
        public void TeleportToPlayer(Client sender, int id)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do teleportu.");
                return;
            }
            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }
            sender.Position = new Vector3(controller.Client.Position.X - 5f, controller.Client.Position.Y, controller.Client.Position.Z + 1f);
        }

        [Command("spec", "~y~ UŻYJ ~w~ /spec [id]")]
        public void SpectatePlayer(Client sender, int id)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do obserwowania.");
                return;
            }

            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }
            NAPI.Player.SetPlayerToSpectatePlayer(sender, controller.Client);
            sender.SendInfo($"Włączono obserwowanie na gracza {controller.CharacterEntity.FormatName}");
        }

        [Command("specoff")]
        public void TurnOffSpectating(Client sender)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do obserwowania.");
                return;
            }

            NAPI.Player.SetPlayerToSpectator(sender);
            sender.SendInfo("Wyłączono obserwowanie.");
        }

        [Command("addspec", "~y~ UŻYJ ~w~ /addspec [id]", Description = "Polecenie ustawia wybranemu graczowi specowanie na nas.")]
        public void AddSpectator(Client sender, int id)
        {
            if (!sender.HasRank(ServerRank.Support3))
            {
                sender.SendWarning("Nie posiadasz uprawnień do ustawienia obserwowania.");
                return;
            }
            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }
            NAPI.Player.SetPlayerToSpectator(controller.Client);
            sender.SendInfo("Włączono obserwowanie.");
        }

        [Command("kick", "~y~ UŻYJ ~w~ /kick [id] (powod)", GreedyArg = true)]
        public void KickPlayer(Client sender, int id, string reason = "")
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do wyrzucenia gracza.");
                return;
            }
            AccountEntity accountEntity = EntityHelper.GetAccountByServerId(id);
            if (accountEntity == null)
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
                return;
            }
            accountEntity.Kick(sender.GetAccountEntity(), reason);
        }

        [Command("fly")]
        public void StartFying(Client sender)
        {
            AccountEntity senderAccount = sender.GetAccountEntity();
            if (!sender.HasRank(ServerRank.Support5))
            {
                sender.SendWarning("Nie posiadasz uprawnień do latania.");
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(sender, RemoteEvents.PlayerFreeCamRequested);

            if (senderAccount.CharacterEntity.IsFlying)
            {
                senderAccount.CharacterEntity.IsFlying = false;
                sender.SendInfo("Wyłączono latanie.");
                return;
            }

            senderAccount.CharacterEntity.IsFlying = true;
            sender.SendInfo("Włączono latanie.");
        }

        [Command("god")]
        public void SetPlayerInvicible(Client sender)
        {
            if (!sender.HasRank(ServerRank.AdministratorTechniczny))
            {
                sender.SendWarning("Nie posiadasz uprawnień do ustawienia nieśmiertelności.");
                return;
            }
            if (sender.Invincible)
            {
                sender.Invincible = false;
                sender.SendInfo("Wyłączono nieśmiertelność.");
            }
            else
            {
                sender.Invincible = true;
                sender.SendInfo("Włączono nieśmiertelność.");
            }
        }

        [Command("inv")]
        public void SetPlayerInvisible(Client sender)
        {
            if (!sender.HasRank(ServerRank.AdministratorTechniczny)
            )
            {
                sender.SendInfo("Nie posiadasz uprawnień do ustawienia niewidzialności.");
                return;
            }

            if (sender.Transparency == 0)
            {
                sender.Transparency = 1;
                sender.SendInfo("Wyłączono niewidzialność.");
            }
            else
            {
                sender.Transparency = 0;
                sender.SendInfo("Włączono niewidzialności.");
            }
        }

        [Command("hp")]
        public void SetPlayerHealth(Client sender, byte health, int id = -1)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendInfo("Nie posiadasz uprawnień do ustawienia poziomu zdrowia.");
                return;
            }

            var getter = EntityHelper.GetAccountByServerId(id) ?? sender.GetAccountEntity();
            getter.CharacterEntity.Health = health;
        }

        [Command("savepos", "~y~ UŻYJ ~w~ /savepos [nazwa]", GreedyArg = true)]
        public void SaveCustomPosition(Client sender, string name)
        {
            if (!sender.HasRank(ServerRank.Support))
            {
                sender.SendWarning("Nie posiadasz uprawnień do zapisywania pozycji.");
                return;
            }

            string path = Path.Combine(Utils.WorkingDirectory, @"Files\CustomPositions.txt");

            if (!Directory.Exists(Path.Combine(Utils.WorkingDirectory, @"\Files\")))
            {
                Directory.CreateDirectory(Path.Combine(Utils.WorkingDirectory, @"\Files\"));

                Colorful.Console.WriteLine($@"[{nameof(AdminCoreScript)}][File] Created path {Utils.WorkingDirectory}\Files\", Color.CornflowerBlue);
            }

            if (!File.Exists(path))
            {
                File.Create(path);

                Colorful.Console.WriteLine($@"[{nameof(AdminCoreScript)}][File] Created file {path}", Color.CornflowerBlue);
            }

            List<string> data = File.ReadAllLines(path).ToList();

            data.Add($"[{DateTime.Now}] {name} Pos: {sender.Position} Rot: {sender.Rotation} Autor: {sender.GetAccountEntity().DbModel.Name}");

            File.WriteAllLines(path, data);
            sender.SendInfo($"Zapisano pozycję: {name}");
        }
    }
}