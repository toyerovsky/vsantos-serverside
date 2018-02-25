/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Entities;
using Serverside.Entities.Core;

namespace Serverside.Core.Scripts
{
    public class ScoreBoardScript : Script
    {
        //By MissMelisa
        private DateTime _mLastTick = DateTime.Now;

        public ScoreBoardScript()
        {
            CharacterEntity.CharacterLoggedIn += RPLogin_OnPlayerLogin;
        }

        private void Event_OnPlayerDisconnected(Client player, byte type, string reason)
        {
            NAPI.ClientEvent.TriggerClientEventForAll("playerlist_leave", player.SocialClubName);
        }

        private void Event_OnPlayerConnected(Client player)
        {
            var list = new List<string>();
            foreach (var ply in EntityManager.GetAccounts())
            {
                var dic = new Dictionary<string, object>
                {
                    {"socialClubName", ply.Value.Client.SocialClubName},
                    {"serverId", ply.Value.ServerId},
                    {"Name", ply.Value.Client.Name},
                    {"ping", ply.Value.Client.Ping}
                };
                list.Add(JsonConvert.SerializeObject(dic));
            }

            NAPI.ClientEvent.TriggerClientEvent(player, "playerlist", list);
        }

        private void RPLogin_OnPlayerLogin(Client sender, CharacterEntity character)
        {
            NAPI.ClientEvent.TriggerClientEventForAll("playerlist_join", sender.SocialClubName, character.FormatName);
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "playerlist_pings")
            {
                var list = new List<string>();
                foreach (var ply in EntityManager.GetAccounts())
                {
                    var dic = new Dictionary<string, object>
                    {
                        {"socialClubName", ply.Value.Client.SocialClubName},
                        {"serverId", ply.Value.ServerId},
                        {"ping", ply.Value.Client.Ping}
                    };
                    list.Add(JsonConvert.SerializeObject(dic));
                }
                NAPI.ClientEvent.TriggerClientEvent(sender, "playerlist_pings", list);
            }
        }

        private void API_onUpdate()
        {
            if ((DateTime.Now - _mLastTick).TotalMilliseconds >= 1000)
            {
                _mLastTick = DateTime.Now;

                var changedNames = new List<string>();
                foreach (var player in NAPI.Pools.GetAllPlayers())
                {
                    string lastName = player.GetData("playerlist_lastname");

                    if (lastName == null)
                    {
                        player.SetData("playerlist_lastname", player.Name);
                        continue;
                    }

                    if (lastName != player.Name)
                    {
                        player.SetData("playerlist_lastname", player.Name);

                        var dic = new Dictionary<string, object>
                        {
                            {"socialClubName", player.SocialClubName},
                            {"newName", player.Name}
                        };
                        changedNames.Add(JsonConvert.SerializeObject(dic));
                    }
                }

                if (changedNames.Count > 0)
                {
                    NAPI.ClientEvent.TriggerClientEventForAll("playerlist_changednames", changedNames);
                }
            }
        }
    }
}
