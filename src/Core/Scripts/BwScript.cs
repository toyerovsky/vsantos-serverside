/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys³aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Timers;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Entities;

namespace Serverside.Core.Scripts
{
    public sealed class BwScript : Script
    {
        public BwScript()
        {
            Event.OnResourceStart += API_onResourceStart;
            Event.OnPlayerDeath += Event_OnPlayerDeath;
        }

        private void Event_OnPlayerDeath(Client sender, Client killer, uint reason, CancelEventArgs cancel)
        {
            cancel.Spawn = false;

            var player = sender.GetAccountEntity();

            var playerCharacter = player.CharacterEntity.DbModel;

            DateTime reviveDate = DateTime.Now.AddMinutes(
                playerCharacter.MinutesToRespawn > 0 ? playerCharacter.MinutesToRespawn : GetTimeToRespawn(reason));

            NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", false);

            ChatScript.SendMessageToPlayer(sender, "Zosta³eœ brutalnie zraniony, aby uœmierciæ swoj¹ postaæ wpisz: /akceptujsmierc", ChatMessageType.ServerInfo);

            player.CharacterEntity.CanTalk = false;
            sender.SetData("CharacterBW", GetTimeToRespawn(reason));

            Timer timer = new Timer(1000);
            timer.Start();

            Event.OnPlayerDisconnected += (client, type, s) =>
            {
                if (sender == client)
                    timer.Dispose();
            };

            timer.Elapsed += (s, e) =>
            {
                if (sender.IsNull || !sender.Exists || !playerCharacter.IsAlive)
                {
                    timer.Dispose();
                }

                //Zdejmowanie BW
                if (!sender.HasData("CharacterBW"))
                {
                    playerCharacter.MinutesToRespawn = 0;

                    playerCharacter.HitPoints = 20;
                    NAPI.Player.SetPlayerHealth(sender, 20);

                    NAPI.Player.SpawnPlayer(player.Client, new Vector3(sender.Position.X, sender.Position.Y, sender.Rotation.Z));
                    NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", true);

                    ChatScript.SendMessageToPlayer(sender, "Twoje BW zosta³o anulowane.", ChatMessageType.ServerInfo);
                    player.CharacterEntity.CanTalk = true;
                    timer.Dispose();
                }
                //Odliczanie
                else if (reviveDate.CompareTo(DateTime.Now) > 0)
                {
                    if (DateTime.Now.Second == 0)
                        sender.SetData("CharacterBW", (reviveDate - DateTime.Now).Minutes);

                    string secondsToShow = (reviveDate - DateTime.Now).Seconds.ToString();
                    secondsToShow = secondsToShow.PadLeft(2, '0');

                    NAPI.ClientEvent.TriggerClientEvent(sender, "BWTimerTick", $"{(reviveDate - DateTime.Now).Minutes}: {secondsToShow}");
                }
                //Koniec BW
                else
                {
                    playerCharacter.HitPoints = 20;
                    NAPI.Player.SetPlayerHealth(sender, 20);

                    NAPI.Player.SpawnPlayer(player.Client, new Vector3(sender.Position.X, sender.Position.Y, sender.Rotation.Z));
                    NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", true);

                    playerCharacter.MinutesToRespawn = 0;

                    player.CharacterEntity.CanTalk = true;
                    sender.ResetData("CharacterBW");

                    timer.Dispose();
                }
                player.CharacterEntity.Save();
            };
        }

        private void API_onResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(BwScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        private int GetTimeToRespawn(uint reason)
        {
            //TODO: Wyznaczyæ czasy odrodzenia w zale¿noœci od broni
            return 5;
        }

        #region Komendy

        [Command("akceptujsmierc")]
        public void CharacterKill(Client sender)
        {
            Event.OnChatMessage += Handler;
            sender.TriggerEvent("SendNotification", "Raz uœmierconej postaci nie mo¿na odblokowaæ. Aby kontynuowaæ wpisz \"akceptujsmierc\"");

            void Handler(Client o, string command, CancelEventArgs cancel)
            {
                if (o == sender && command == "akceptujsmierc")
                {
                    cancel.Cancel = true;
                    var player = sender.GetAccountEntity();
                    player.CharacterEntity.DbModel.IsAlive = false;
                    player.Save();
                    sender.Kick("CK");
                }
            }
        }

        [Command("bw", "~y~U¯YJ: ~w~ /bw [id]")]
        public void SetPlayerBw(Client sender, int id)
        {
            if (EntityManager.GetAccountByServerId(id) != null)
            {
                Client getter = EntityManager.GetAccountByServerId(id).Client;
                getter.TriggerEvent("ToggleHud", true);
                getter.ResetData("CharacterBW");
            }
            else
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
            }
        }
        #endregion
    }
}
