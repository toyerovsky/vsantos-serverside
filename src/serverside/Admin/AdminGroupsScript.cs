/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
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
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia grupy.");
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
                sender.SendError("Wprowadzony kolor jest nieprawidłowy.");
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
                    sender.SendInfo($"Stworzyłeś grupę {group.GetColoredName()}.");
                }
                else
                {
                    boss.Client.SendError("Nie posiadasz wolnych slotów grupowych.");
                    if (boss.Client != sender)
                        sender.SendError(
                            $"Gracz: {boss.Client.Name} nie posiada wolnych slotów grupowych.");
                }
            }
            else
            {
                sender.SendError("Nie znaleziono gracza o podanym Id.");
            }
        }

        [Command("wejdzgrupa")]
        public void JoinGroup(Client sender, long groupId)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.GameMaster)
            {
                sender.SendError("Nie posiadasz uprawnień do ustawienia wchodzenia do grupy.");
                return;
            }

            if (EntityHelper.GetGroup(groupId) != null)
            {
                AccountEntity player = sender.GetAccountEntity();

                GroupEntity group = EntityHelper.GetGroup(groupId);

                if (group.GetWorkers().Any(p => p.Character == player.CharacterEntity.DbModel))
                {
                    sender.SendError("Jesteś już w tej grupie.");
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

                    sender.SendInfo($"Wszedłeś do grupy {group.GetColoredName()}.");
                }
                else
                {
                    sender.SendError("Nie posiadasz wolnych slotów grupowych.");
                }
            }
            else
            {
                sender.SendError("Nie znaleziono grupy o podanym Id.");
            }
        }
    }
}
