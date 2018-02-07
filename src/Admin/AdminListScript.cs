/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Admin.Enums;
using Serverside.Admin.Structs;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Entities;
using Serverside.Entities.Core;

namespace Serverside.Admin
{
    public class AdminListScript : Script
    {
        private List<AccountEntity> AdminsOnDuty { get; set; } = new List<AccountEntity>();
        private List<ReportData> CurrentReports { get; set; } = new List<ReportData>();

        public AdminListScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(AdminListScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        private void OnClientEventTriggerHandler(Client sender, string eventName, params object[] arguments)
        {
            /* Arguments
             * args[0] string reportType
             * args[1] string content
             * args[2] string accusedId = ""
             */
            if (eventName == "OnPlayerSendReport")
            {
                ReportData data = new ReportData
                {
                    Type = (ReportType)Enum.Parse(typeof(ReportType), arguments[0].ToString().Replace(' ', '_')),
                    Content = arguments[1].ToString(),
                    Accused = arguments[2].ToString() != ""
                        ? EntityManager.GetAccountByServerId(Convert.ToInt32(arguments[2]))
                        : null,
                    Sender = sender.GetAccountEntity()
                };

                CurrentReports.Add(data);
            }
        }



        [Command("a")]
        public void ShowAdministratorsList(Client sender)
        {
            if (AdminsOnDuty.Count == 0)
            {
                sender.Notify("Obecnie nie ma administratorów na służbie.");
                return;
            }

            sender.TriggerEvent("ShowAdminsOnDuty", AdminsOnDuty.Select(x => new
            {
                x.ServerId,
                ForumName = x.DbModel.Name,
                Rank = x.DbModel.ServerRank.ToString(),
            }).OrderBy(x => x.Rank));
        }

        [Command("listreports", Alias = "lr")]
        public void ShowCurrentReports(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do przeglądania raportów.");
                return;
            }

            sender.TriggerEvent("ShowAdminReportMenu", JsonConvert.SerializeObject(CurrentReports.Select(x => new
            {
                SenderName = x.Sender.CharacterEntity.FormatName,
                SenderId = x.Sender.ServerId.ToString(),
                AccusedName = x.Accused?.CharacterEntity.FormatName ?? "",
                AccusedId = x.Accused?.ServerId.ToString() ?? "",
                x.Content,
                ReportType = x.Type.ToString().Replace('_', ' ')
            })));
        }

        [Command("report")]
        public void SendReport(Client sender)
        {
            sender.TriggerEvent("ShowReportMenu");
        }

        [Command("aduty")]
        public void EnterAdminDuty(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.Support)
            {
                sender.Notify("Nie posiadasz uprawnień do służby administracyjnej.");
                return;
            }

            var player = sender.GetAccountEntity();

            if (AdminsOnDuty.Any(a => a.AccountId == player.AccountId))
            {
                AdminsOnDuty.Remove(player);
                NAPI.Chat.SendChatMessageToPlayer(sender, $"Zszedłeś ze służby ~{sender.GetRankColor().ToHex()}~ {player.DbModel.ServerRank.ToString().Where(char.IsLetter)} ~w~ życzymy miłej gry.");
            }
            else
            {
                AdminsOnDuty.Add(player);
                NAPI.Chat.SendChatMessageToPlayer(sender, $"Wszedłeś na służbę ~{sender.GetRankColor().ToHex()}~ {player.DbModel.ServerRank.ToString().Where(char.IsLetter)} ~w~ życzymy cierpliwości.");
            }
        }
    }
}