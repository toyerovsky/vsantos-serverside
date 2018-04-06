/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Exceptions;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using Color = GTANetworkAPI.Color;

namespace VRP.Serverside.Admin
{
    public class AdminGroupsScript : Script
    {
        [Command("stworzgrupe")]
        public void CreateGroup(Client sender, int bossId, GroupType type, string name, string tag, string hexColor)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster2)
            {
                sender.Notify("Nie posiadasz uprawnień do tworzenia grupy.", NotificationType.Warning);
                return;
            }

            name = name.Replace('_', ' ');

            Color color;
            try
            {
                color = hexColor.ToColor();
            }
            catch (ColorConvertException)
            {
                sender.Notify("Wprowadzony kolor jest nieprawidłowy.", NotificationType.Error);
                return;
            }

            if (EntityHelper.GetAccountByServerId(bossId) != null)
            {
                AccountEntity boss = EntityHelper.GetAccountByServerId(bossId);

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
                    sender.Notify($"Stworzyłeś grupę {group.GetColoredName()}.", NotificationType.Info);
                }
                else
                {
                    boss.Client.Notify("Nie posiadasz wolnych slotów grupowych.", NotificationType.Error);
                    if (boss.Client != sender)
                        sender.Notify(
                            $"Gracz: {boss.Client.Name} nie posiada wolnych slotów grupowych.", NotificationType.Error);
                }
            }
            else
            {
                sender.Notify("Nie znaleziono gracza o podanym Id.", NotificationType.Error);
            }
        }

        [Command("wejdzgrupa")]
        public void JoinGroup(Client sender, long groupId)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.Notify("Nie posiadasz uprawnień do ustawienia wchodzenia do grupy.", NotificationType.Error);
                return;
            }

            if (EntityHelper.GetGroup(groupId) != null)
            {
                AccountEntity player = sender.GetAccountEntity();

                GroupEntity group = EntityHelper.GetGroup(groupId);

                if (group.GetWorkers().Any(p => p.Character == player.CharacterEntity.DbModel))
                {
                    sender.Notify("Jesteś już w tej grupie.", NotificationType.Error);
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

                    sender.Notify($"Wszedłeś do grupy {group.GetColoredName()}.", NotificationType.Info);
                }
                else
                {
                    sender.Notify("Nie posiadasz wolnych slotów grupowych.", NotificationType.Error);
                }
            }
            else
            {
                sender.Notify("Nie znaleziono grupy o podanym Id.", NotificationType.Error);
            }
        }
    }
}
