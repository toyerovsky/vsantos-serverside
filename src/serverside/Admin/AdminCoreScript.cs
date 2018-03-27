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
using VRP.Core.Tools;
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
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Adminadministrator3)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawiania rang.");
                return;
            }

            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }

            controller.DbModel.ServerRank = rank;
            controller.Save();
            sender.Notify($"Nadałeś {controller.CharacterEntity.FormatName} ({controller.DbModel.Name}) rangę {controller.DbModel.ServerRank.ToString()}");
        }

        [Command("tpc", "~y~ UŻYJ ~w~ /tpc [x] [y] [z]")]
        public void TeleportToCords(Client sender, float x, float y, float z)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do teleportu na koordynaty.");
                return;
            }
            sender.Position = new Vector3(x, y, z);
            sender.Notify($"Teleportowałeś się na X:{x} Y:{y} Z:{z}");
        }

        [Command("tpmap", "~y~ UŻYJ ~w~ /tpmap")]
        public void TeleportToWaypoint(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do teleportu na waypoint.");
                return;
            }
            Action<Vector3> teleportAction = position => sender.Position = position;

            sender.SetData("WaypointVectorHandler", teleportAction);
            sender.TriggerEvent("GetWaypointPosition");

        }

        [Command("tp", "~y~ UŻYJ ~w~ /tp [id]")]
        public void TeleportToPlayer(Client sender, int id)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do teleportu.");
                return;
            }
            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }
            sender.Position = new Vector3(controller.Client.Position.X - 5f, controller.Client.Position.Y, controller.Client.Position.Z + 1f);
        }

        [Command("spec", "~y~ UŻYJ ~w~ /spec [id]")]
        public void SpectatePlayer(Client sender, int id)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do obserwowania.");
                return;
            }
            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }
            NAPI.Player.SetPlayerToSpectatePlayer(sender, controller.Client);
            sender.Notify($"Włączono obserwowanie na gracza {controller.CharacterEntity.FormatName}");
        }

        [Command("specoff")]
        public void TurnOffSpectating(Client sender)
        {
            NAPI.Player.SetPlayerToSpectator(sender);
            sender.Notify("~r~ ~h~Wyłączono ~w~obserwowanie.");
        }

        [Command("addspec", "~y~ UŻYJ ~w~ /addspec [id]", Description = "Polecenie ustawia wybranemu graczowi specowanie na nas.")]
        public void AddSpectator(Client sender, int id)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support3)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia obserwowania.");
                return;
            }
            AccountEntity controller = EntityHelper.GetAccountByServerId(id);
            if (controller == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }
            NAPI.Player.SetPlayerToSpectator(controller.Client);
            sender.Notify("~g~ ~h~Włączono ~w~obserwowanie.");
        }

        [Command("kick", "~y~ UŻYJ ~w~ /kick [id] (powod)", GreedyArg = true)]
        public void KickPlayer(Client sender, int id, string reason = "")
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia obserwowania.");
                return;
            }
            AccountEntity accountEntity = EntityHelper.GetAccountByServerId(id);
            if (accountEntity == null)
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
                return;
            }
            accountEntity.Kick(sender.GetAccountEntity(), reason);
        }

        [Command("fly")]
        public void StartFying(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support5)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia latania.");
                return;
            }

            if (sender.HasData("FlyState"))
            {
                sender.ResetData("FlyState");
                NAPI.ClientEvent.TriggerClientEvent(sender, "FreeCamStop");
                sender.Notify("~r~ ~h~Wyłączono ~w~latanie.");
                return;
            }

            sender.SetData("FlyState", true);
            NAPI.ClientEvent.TriggerClientEvent(sender, "FreeCamStart", sender.Position);
            sender.Notify("~g~ ~h~Włączono ~w~latanie.");
        }

        [Command("god")]
        public void SetPlayerInvicible(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia nieśmiertelności.");
                return;
            }
            if (sender.Invincible)
            {
                sender.Invincible = false;
                sender.Notify("~r~Wyłączono ~w~nieśmiertelność.");
            }
            else
            {
                sender.Invincible = true;
                sender.Notify("~g~~h~Włączono ~w~nieśmiertelność.");
            }
        }

        [Command("inv")]
        public void SetPlayerInvisible(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia niewidzialności.");
                return;
            }

            if (sender.Transparency == 0)
            {
                sender.Transparency = 1;
                sender.Notify("~r~Wyłączono ~w~niewidzialności.");
            }
            else
            {
                sender.Transparency = 0;
                sender.Notify("~g~~h~Włączono ~w~niewidzialności.");
            }
        }

        [Command("savepos", "~y~ UŻYJ ~w~ /savepos [nazwa]", GreedyArg = true)]
        public void SaveCustomPosition(Client sender, string name)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do zapisywania pozycji.");
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
            sender.Notify($"Zapisano pozycję: {name}");
        }
    }
}