/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using GTANetworkAPI;
using Serverside.Admin.Enums;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Entities;
using Serverside.Entities.Core;
using Serverside.Groups.Enums;

namespace Serverside.Admin
{
    public class AdminGroupsScript : Script
    {
        public AdminGroupsScript()
        {
            Tools.ConsoleOutput($"[{nameof(AdminGroupsScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        [Command("stworzgrupe", GreedyArg = true)]
        public void CreateGroup(Client sender, int bossId, GroupType type, string name, string tag, string hexColor)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster2)
            {
                sender.Notify("Nie posiadasz uprawnień do tworzenia grupy.");
                return;
            }

            Color color;
            try
            {
                color = hexColor.ToColor();
            }
            catch (Exception e)
            {
                sender.Notify("Wprowadzony kolor jest nieprawidłowy.");
                Tools.ConsoleOutput("[AdminGroupsScript] Nieprawidłowy kolor [RPAdminGroups]", ConsoleColor.Red);
                Tools.ConsoleOutput(e.Message, ConsoleColor.Red);
                return;
            }

            if (EntityManager.GetAccountByServerId(bossId) != null)
            {
                var boss = EntityManager.GetAccountByServerId(bossId);
                
                if (boss.CharacterEntity.DbModel.Workers.Count < 3)
                {
                    GroupEntity group = GroupEntity.Create(name, tag, type, color);
                    group.GetWorkers().Add(new WorkerModel
                    {
                        Group = group.DbModel,
                        Character = boss.CharacterEntity.DbModel,
                        ChatRight = true,
                        DoorsRight = true,
                        OfferFromWarehouseRight = true,
                        PaycheckRight = true,
                        RecrutationRight = true,
                        DutyMinutes = 0,
                        Salary = 0
                    });
                    group.Save();
                    sender.Notify($"Stworzyłeś grupę {group.GetColoredName()}.");
                }
                else
                {
                    boss.Client.Notify("Nie posiadasz wolnych slotów grupowych.");
                    if (boss.Client != sender)
                        sender.Notify(
                            $"Gracz: {boss.Client.Name} nie posiada wolnych slotów grupowych.");
                }
            }
            else
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.");
            }
        }

        [Command("wejdzgrupa")]
        public void JoinGroup(Client sender, long groupId)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia wchodzenia do grupy.");
                return;
            }

            if (EntityManager.GetGroup(groupId) != null)
            {
                var player = sender.GetAccountEntity();

                GroupEntity group = EntityManager.GetGroup(groupId);

                if (group.GetWorkers().Any(p => p.Character == player.CharacterEntity.DbModel))
                {
                    sender.Notify("Jesteś już w tej grupie.");
                    return;
                }

                if (player.CharacterEntity.DbModel.Workers.Count < 3)
                {
                    group.GetWorkers().Add(new WorkerModel
                    {
                        Group = group.DbModel,
                        Character = sender.GetAccountEntity().CharacterEntity.DbModel,
                        ChatRight = true,
                        DoorsRight = true,
                        OfferFromWarehouseRight = true,
                        PaycheckRight = true,
                        RecrutationRight = true,
                        DutyMinutes = 0,
                        Salary = 0
                    });
                    group.Save();

                    sender.Notify($"Wszedłeś do grupy {group.GetColoredName()}.");
                }
                else
                {
                    sender.Notify("Nie posiadasz wolnych slotów grupowych.");
                }
            }
            else
            {
                sender.Notify("Nie znaleziono grupy o podanym Id.");
            }
        }
    }
}
