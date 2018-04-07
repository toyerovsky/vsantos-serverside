/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemys³aw Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Scripts
{
    public class BwScript : Script
    {
        public void Event_OnPlayerDeath(Client sender, Client killer, WeaponHash reason)
        {
            AccountEntity player = sender.GetAccountEntity();

            CharacterModel playerCharacter = player.CharacterEntity.DbModel;

            DateTime reviveDate = DateTime.Now.AddMinutes(
                playerCharacter.MinutesToRespawn > 0 ? playerCharacter.MinutesToRespawn : GetTimeToRespawn(reason));

            NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", false);

            sender.SendWarning("Zosta³eœ brutalnie zraniony, aby uœmierciæ swoj¹ postaæ wpisz: /akceptujsmierc");

            player.CharacterEntity.CanTalk = false;
            sender.SetData("CharacterBW", GetTimeToRespawn(reason));

            Timer timer = new Timer(1000);
            timer.Start();

            AccountEntity.AccountLoggedOut += (client, account) =>
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

                    playerCharacter.Health = 20;
                    NAPI.Player.SetPlayerHealth(sender, 20);

                    NAPI.Player.SpawnPlayer(player.Client, new Vector3(sender.Position.X, sender.Position.Y, sender.Rotation.Z));
                    NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", true);

                    sender.SendInfo("Twoje BW zosta³o anulowane.");
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
                    playerCharacter.Health = 20;
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

        private int GetTimeToRespawn(WeaponHash reason)
        {
            //TODO: Wyznaczyæ czasy odrodzenia w zale¿noœci od broni
            return 5;
        }

        #region Komendy

        [Command("akceptujsmierc")]
        public void CharacterKill(Client sender)
        {
            if (sender.HasData("CharacterBW"))
            {
                AccountEntity player = sender.GetAccountEntity();
                player.CharacterEntity.DbModel.IsAlive = false;
                player.Save();
                sender.Kick("CK");
            }
        }

        [Command("bw", "~y~U¯YJ: ~w~ /bw [id]")]
        public void SetPlayerBw(Client sender, int id)
        {
            if (EntityHelper.GetAccountByServerId(id) != null)
            {
                Client getter = EntityHelper.GetAccountByServerId(id).Client;
                getter.TriggerEvent("ToggleHud", true);
                getter.ResetData("CharacterBW");
            }
            else
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
            }
        }
        #endregion
    }
}
