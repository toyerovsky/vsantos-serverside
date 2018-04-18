/* Copyright (C) Przemys³aw Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Scripts
{
    public class BwScript : Script
    {
        public void Event_OnPlayerDeath(Client sender, Client killer, WeaponHash reason)
        {
            CharacterEntity playerCharacter = sender.GetAccountEntity().CharacterEntity;

            DateTime reviveDate = DateTime.Now.AddMinutes(
                playerCharacter.DbModel.MinutesToRespawn > 0 ? playerCharacter.DbModel.MinutesToRespawn : GetTimeToRespawn(reason));

            sender.SendWarning("Zosta³eœ brutalnie zraniony, aby uœmierciæ swoj¹ postaæ wpisz: /akceptujsmierc");

            playerCharacter.CanTalk = false;
            sender.SetData("CharacterBW", GetTimeToRespawn(reason));

            Timer timer = new Timer(1000);
            timer.Start();

            sender.TriggerEvent(RemoteEvents.PlayerBwTimerRequested, 1000, reviveDate);

            AccountEntity.AccountLoggedOut += (client, account) =>
            {
                if (sender == client)
                    timer.Dispose();
            };

            timer.Elapsed += (s, e) =>
            {
                if (!sender.HasData("CharacterBW")) // Zdejmowanie BW
                {
                    NAPI.Player.SpawnPlayer(sender, playerCharacter.Position);
                    playerCharacter.SetBw(0);
                    sender.TriggerEvent(RemoteEvents.PlayerBwTimerDestroyRequested);
                    sender.SendInfo("Twoje BW zosta³o anulowane.");
                    timer.Dispose();
                }
                else if (reviveDate.CompareTo(DateTime.Now) > 0) // Odliczanie
                {
                    if (DateTime.Now.Second == 0)
                        sender.SetData("CharacterBW", (reviveDate - DateTime.Now).Minutes);
                }
                else // Koniec BW
                {
                    playerCharacter.SetBw(0);
                    NAPI.Player.SpawnPlayer(sender, new Vector3(sender.Position.X, sender.Position.Y, sender.Rotation.Z));
                    NAPI.ClientEvent.TriggerClientEvent(sender, "ToggleHud", true);
                    sender.ResetData("CharacterBW");
                    timer.Dispose();
                }
                playerCharacter.Save();
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
