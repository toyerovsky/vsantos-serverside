/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Tools;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using AnimationFlags = VRP.Serverside.Core.Extensions.AnimationFlags;

namespace VRP.Serverside.Economy.Groups
{
    public class GroupsScript : Script
    {
        #region Character commands

        [Command("prowadz", "~y~ UŻYJ ~w~ /pro(wadz) (id)", Alias = "pro")]
        public void ForcePlayerToGo(Client sender, int id = -1)
        {
            if (sender.GetAccountEntity().CharacterEntity.OnDutyGroup is Police group)
            {
                if (!group.CanPlayerDoPolice(sender.GetAccountEntity()))
                {
                    sender.Notify("Twoja postać nie posiada uprawnień do używania prowadzenia.", NotificationType.Warning);
                    return;
                }

                AccountEntity getter = null;
                float distance;
                if (id != -1)
                {
                    getter = EntityHelper.GetAccountByServerId(id);
                    if (getter != null)
                    {
                        distance = getter.Client.Position.DistanceTo(sender.Position);
                        if (distance > 3 || distance < -3)
                        {
                            sender.Notify("Podany gracz znajduje się za daleko.", NotificationType.Warning);
                            return;
                        }
                    }
                    else
                    {
                        sender.Notify("Nie znaleziono gracza o podanym Id.", NotificationType.Error);
                        return;
                    }
                }
                else
                {
                    Client nearestPlayer = sender.Position.GetNearestPlayer();
                    if (nearestPlayer != null)
                    {
                        distance = nearestPlayer.Position.DistanceTo2D(sender.Position);
                        if (distance < 3 && distance > -3)
                        {
                            getter = nearestPlayer.GetAccountEntity();
                        }
                    }
                }

                if (getter == null) return;
                getter.Client.FreezePosition = true;
                NAPI.Player.PlayPlayerAnimation(getter.Client, (int)AnimationFlags.Loop, "mp_arresting", "walk");
            }

        }

        [Command("kajdanki", "~y~ UŻYJ ~w~ /kaj(danki) (id)", Alias = "kaj")]
        public void CuffPlayer(Client sender, int id = -1)
        {
            GroupEntity group = sender.GetAccountEntity().CharacterEntity.OnDutyGroup;
            if (group == null) return;
            if (group.DbModel.GroupType != GroupType.Police || !((Police)group).CanPlayerDoPolice(sender.GetAccountEntity()))
            {
                sender.Notify("Twoja grupa, bądź postać nie posiada uprawnień do używania kajdanek.", NotificationType.Warning);
                return;
            }

            AccountEntity getter = null;
            float distance;
            if (id != -1)
            {
                getter = EntityHelper.GetAccountByServerId(id);
                if (getter != null)
                {
                    distance = getter.Client.Position.DistanceTo2D(sender.Position);
                    if (distance > 3 || distance < -3)
                    {
                        sender.Notify("Podany gracz znajduje się za daleko.", NotificationType.Warning);
                        return;
                    }
                }
                else
                {
                    sender.Notify("Nie znaleziono gracza o podanym Id.", NotificationType.Error);
                    return;
                }
            }
            else
            {
                Client nearestPlayer = sender.Position.GetNearestPlayer();
                if (nearestPlayer != null)
                {
                    distance = nearestPlayer.Position.DistanceTo2D(sender.Position);
                    if (distance < 3 && distance > -3)
                    {
                        getter = nearestPlayer.GetAccountEntity();
                    }
                }
            }

            if (getter == null) return;
            NAPI.Player.PlayPlayerAnimation(getter.Client, (int)AnimationFlags.Loop, "arrest", "arrest_fallback_high_cop");
            NAPI.Player.PlayPlayerAnimation(sender, (int)(AnimationFlags.Cancellable | AnimationFlags.AllowPlayerControl), "rcmme_amanda1", "arrest_ama");
        }

        [Command("gduty")]
        public void EnterDuty(Client sender, byte slot)
        {
            Timer dutyTimer = new Timer(60000);

            AccountEntity player = sender.GetAccountEntity();
            if (player.CharacterEntity.OnDutyGroup != null)
            {
                sender.Notify(
                    $"Zszedłeś ze służby grupy: {player.CharacterEntity.OnDutyGroup.GetColoredName()}", NotificationType.Info);

                player.CharacterEntity.OnDutyGroup.PlayersOnDuty.Remove(player);
                player.CharacterEntity.OnDutyGroup = null;
                sender.ResetNametagColor();
                sender.Nametag = $"[{player.ServerId}] {player.CharacterEntity.FormatName}";
                dutyTimer.Stop();
                dutyTimer.Dispose();
            }
            else
            {
                if (!ValidationHelper.IsGroupSlotValid(slot))
                {
                    sender.Notify("Podany slot grupy nie jest poprawny.", NotificationType.Error);
                    return;
                }

                if (sender.TryGetGroupByUnsafeSlot(Convert.ToInt16(slot), out GroupEntity group))
                {
                    WorkerModel worker =
                        group.GetWorkers().Single(x => x.Character.Id == player.CharacterEntity.DbModel.Id);

                    dutyTimer.Start();

                    dutyTimer.Elapsed += (o, args) =>
                    {
                        worker.DutyMinutes += 1;
                        group.Save();
                    };

                    sender.Nametag = $"( {player.ServerId} ) ( {group.DbModel.Name} ) {sender.Name}";
                    sender.NametagColor = group.DbModel.Color.ToColor();

                    player.CharacterEntity.OnDutyGroup = group;
                    player.CharacterEntity.OnDutyGroup.PlayersOnDuty.Add(player);
                    sender.Notify(
                        $"Wszedłeś na służbę grupy: {group.GetColoredName()}", NotificationType.Info);

                    AccountEntity.AccountLoggedOut += (client, account) =>
                    {
                        if (client == sender) dutyTimer.Dispose();
                    };
                }
                else
                {
                    sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
                }
            }
        }

        [Command("gwyplac")]
        public void TakeMoneyFromGroup(Client sender, short slot, decimal safeMoneyCount)
        {
            if (!ValidationHelper.IsMoneyValid(safeMoneyCount))
            {
                sender.Notify("Podano kwotę gotówki w nieprawidłowym formacie.", NotificationType.Error);
            }

            if (sender.TryGetGroupByUnsafeSlot(slot, out GroupEntity group))
            {
                if (group.CanPlayerTakeMoney(sender.GetAccountEntity()))
                {
                    if (group.HasMoney(safeMoneyCount))
                    {
                        group.RemoveMoney(safeMoneyCount);
                        CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                        character.AddMoney(safeMoneyCount);

                        sender.Notify($"Wypłacono ${safeMoneyCount} z konta grupy {group.GetColoredName()}.", NotificationType.Info);
                    }
                    else
                    {
                        sender.Notify($"Grupa {group.GetColoredName()}, nie posiada tyle pieniędzy na koncie.", NotificationType.Info);
                    }
                }
                else
                {
                    sender.Notify("Nie posiadasz uprawnień do wypłacania gotówki.", NotificationType.Warning);
                }
            }
            else
            {
                sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
            }
        }

        [Command("gwplac")]
        public void PutMoneyIntoGroup(Client sender, short groupSlot, decimal safeMoneyCount)
        {
            if (!ValidationHelper.IsMoneyValid(safeMoneyCount))
            {
                sender.Notify("Podano kwotę gotówki w nieprawidłowym formacie.", NotificationType.Error);
            }

            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group))
            {
                CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                if (character.HasMoney(safeMoneyCount))
                {
                    character.RemoveMoney(safeMoneyCount);
                    group.AddMoney(safeMoneyCount);

                    sender.Notify($"Wpłacono ${safeMoneyCount} na konto grupy {group.GetColoredName()}.", NotificationType.Info);
                }
                else
                {
                    sender.Notify("Nie posiadasz tyle gotówki.", NotificationType.Info);
                }
            }
            else
            {
                sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
            }
        }

        [Command("g")]
        public void ShowGroupMenu(Client sender, byte slot)
        {
            AccountEntity player = sender.GetAccountEntity();
            // Nie wiem czy nie popsuje to czegos gdy grupa jest - do sprawdzenia
            if ((EntityHelper.GetPlayerGroups(player) == null ? true : false))
            {
                sender.Notify("Nie jesteś członkiem żadnej grupy.", NotificationType.Warning);
                return;
            }

            if (!ValidationHelper.IsGroupSlotValid(slot))
            {
                sender.Notify("Podano dane w nieprawidłowym formacie.", NotificationType.Error);
                return;
            }

            if (sender.TryGetGroupByUnsafeSlot(slot, out GroupEntity group))
            {
                sender.TriggerEvent("ShowGroupMenu", JsonConvert.SerializeObject(new
                {
                    group.DbModel.Name,
                    group.DbModel.Tag,
                    group.DbModel.Money,
                    Color = group.DbModel.Color,
                    //To jest raczej kosztowne, ale nie widzę innej opcji
                    PlayersOnLine = JsonConvert.SerializeObject(group.GetWorkers().Where(x => x.Character.Online).Select(w => new
                    {
                        ServerId = EntityHelper.GetAccountByCharacterId(w.Character.Id).ServerId,
                        Name = $"{w.Character.Name} {w.Character.Surname}",
                        Salary = w.Salary,
                        DutyTime = w.DutyMinutes,
                        OnDuty = group.PlayersOnDuty.Contains(EntityHelper.GetAccountByCharacterId(w.Character.Id))
                    }))
                }), group.CanPlayerManageWorkers(player));
            }
            else
            {
                sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
            }
        }

        [Command("gzapros")]
        public void InvitePlayerToGroup(Client sender, short groupSlot, int id)
        {
            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group))
            {
                if (group.CanPlayerManageWorkers(sender.GetAccountEntity()))
                {
                    AccountEntity getter = EntityHelper.GetAccountByServerId(id);
                    if (getter == null)
                    {
                        sender.Notify("Nie znaleziono gracza o podanym Id.", NotificationType.Error);
                        return;
                    }

                    if (group.ContainsWorker(getter))
                    {
                        sender.Notify($"{getter.CharacterEntity.FormatName} już znajduje się w grupie {group.GetColoredName()}", NotificationType.Info);
                        return;
                    }
                    group.AddWorker(getter);
                    getter.Notify($"Zostałeś zaproszony do grupy {group.GetColoredName()} przez {sender.Name}.", NotificationType.Info);
                    sender.Notify($"Zaprosiłeś gracza {getter.Client.Name} do grupy {group.GetColoredName()}.", NotificationType.Info);
                }
                else
                {
                    sender.Notify("Nie posiadasz uprawnień do zarządzania pracownikami.", NotificationType.Warning);
                }
            }
            else
            {
                sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
            }
        }

        [Command("gwypros")]
        public void RemovePlayerFromGroup(Client sender, short groupSlot, int id)
        {
            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group))
            {
                if (group.CanPlayerManageWorkers(sender.GetAccountEntity()))
                {
                    AccountEntity getter = EntityHelper.GetAccountByServerId(id);
                    if (getter == null)
                    {
                        sender.Notify("Nie znaleziono gracza o podanym Id.", NotificationType.Error);
                        return;
                    }

                    if (group.ContainsWorker(getter))
                    {
                        sender.Notify($"{getter.CharacterEntity.FormatName} nie należy do grupy {group.GetColoredName()}", NotificationType.Info);
                        return;
                    }

                    group.RemoveWorker(getter);
                    getter.Notify($"Zostałeś wyproszony z grupy {group.GetColoredName()} przez {sender.Name}.", NotificationType.Info);
                    sender.Notify($"Wyprosiłeś gracza {getter.Client.Name} z grupy {group.GetColoredName()}.", NotificationType.Info);
                }
                else
                {
                    sender.Notify("Nie posiadasz uprawnień do zarządzania pracownikami.", NotificationType.Warning);
                }
            }
            else
            {
                sender.Notify("Nie posiadasz grupy w tym slocie.", NotificationType.Info);
            }
        }

        #endregion
    }
}