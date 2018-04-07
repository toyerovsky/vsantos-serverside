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
using VRP.Core.Enums;
using VRP.Serverside.Admin.Structs;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Admin
{
    public class AdminListScript : Script
    {
        private List<AccountEntity> AdminsOnDuty { get; set; } = new List<AccountEntity>();
        private List<ReportData> CurrentReports { get; set; } = new List<ReportData>();

        [RemoteEvent(RemoteEvents.OnPlayerSendReport)]
        public void OnPlayerSendReportHandler(Client sender, params object[] arguments)
        {
            /* Arguments
             * args[0] string reportType
             * args[1] string content
             * args[2] string accusedId = ""
             */

            ReportData data = new ReportData
            {
                Type = (ReportType)Enum.Parse(typeof(ReportType), arguments[0].ToString().Replace(' ', '_')),
                Content = arguments[1].ToString(),
                Accused = arguments[2].ToString() != ""
                    ? EntityHelper.GetAccountByServerId(Convert.ToInt32(arguments[2]))
                    : null,
                Sender = sender.GetAccountEntity()
            };

            CurrentReports.Add(data);
        }
     

        [Command("a")]
        public void ShowAdministratorsList(Client sender)
        {
            if (AdminsOnDuty.Count == 0)
            {
                sender.Notify("Obecnie nie ma administratorów na służbie.", NotificationType.Info);
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
                sender.SendWarning("Nie posiadasz uprawnień do przeglądania raportów.");
                return;
            }

            sender.TriggerEvent("ShowAdminReportMenu", JsonConvert.SerializeObject(CurrentReports.Select(x => new
            {
                senderName = x.Sender.CharacterEntity.FormatName,
                senderId = x.Sender.ServerId.ToString(),
                accusedName = x.Accused?.CharacterEntity.FormatName ?? "",
                accusedId = x.Accused?.ServerId.ToString() ?? "",
                content = x.Content,
                reportType = x.Type.ToString().Replace('_', ' ')
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
                sender.SendWarning("Nie posiadasz uprawnień do służby administracyjnej.");
               
                return;
            }

            AccountEntity player = sender.GetAccountEntity();

            if (AdminsOnDuty.Any(admin => ReferenceEquals(admin, player)))
            {
                AdminsOnDuty.Remove(player);
                sender.SendInfo($"Zszedłeś ze służby {player.DbModel.ServerRank.GetColoredRankName()} życzymy miłej gry.");
                
            }
            else
            {
                AdminsOnDuty.Add(player);
                sender.SendInfo($"Wszedłeś na służbę {player.DbModel.ServerRank.GetColoredRankName()} życzymy cierpliwości.");
                
            }
        }
    }
}