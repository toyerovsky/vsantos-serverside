/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Validators;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;

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
                    sender.SendWarning("Twoja postać nie posiada uprawnień do używania prowadzenia.");
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
                            sender.SendWarning("Podany gracz znajduje się za daleko.");
                            return;
                        }
                    }
                    else
                    {
                        sender.SendError("Nie znaleziono gracza o podanym Id.");
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
                sender.SendWarning("Twoja grupa, bądź postać nie posiada uprawnień do używania kajdanek.");
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
                        sender.SendWarning("Podany gracz znajduje się za daleko.");
                        return;
                    }
                }
                else
                {
                    sender.SendError("Nie znaleziono gracza o podanym Id.");
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
                sender.SendInfo($"Zszedłeś ze służby grupy: {player.CharacterEntity.OnDutyGroup.GetColoredName()}");

                player.CharacterEntity.OnDutyGroup.PlayersOnDuty.Remove(player);
                player.CharacterEntity.OnDutyGroup = null;
                sender.ResetNametagColor();
                sender.Nametag = $"[{player.ServerId}] {player.CharacterEntity.FormatName}";
                dutyTimer.Stop();
                dutyTimer.Dispose();
            }
            else
            {
                GroupSlotValidator validator = new GroupSlotValidator();
                if (!validator.IsValid(slot))
                {
                    sender.SendError("Podany slot grupy nie jest poprawny.");
                    return;
                }

                if (sender.TryGetGroupByUnsafeSlot(Convert.ToInt16(slot), out GroupEntity group, out WorkerModel worker))
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
                    sender.SendInfo($"Wszedłeś na służbę grupy: {group.GetColoredName()}");

                    AccountEntity.AccountLoggedOut += (client, account) =>
                    {
                        if (client == sender) dutyTimer.Dispose();
                    };
                }
                else
                {
                    sender.SendInfo("Nie posiadasz grupy w tym slocie.");
                }
            }
        }

        [Command("gwyplac")]
        public void TakeMoneyFromGroup(Client sender, short slot, decimal safeMoneyCount)
        {
            MoneyValidator validator = new MoneyValidator();
            if (!validator.IsValid(safeMoneyCount))
            {
                sender.SendError("Podano kwotę gotówki w nieprawidłowym formacie.");
            }

            if (sender.TryGetGroupByUnsafeSlot(slot, out GroupEntity group, out WorkerModel worker))
            {
                if (group.CanPlayerTakeMoney(worker))
                {
                    if (group.HasMoney(safeMoneyCount))
                    {
                        group.RemoveMoney(safeMoneyCount);
                        CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                        character.AddMoney(safeMoneyCount);

                        sender.SendInfo($"Wypłacono ${safeMoneyCount} z konta grupy {group.GetColoredName()}.");
                    }
                    else
                    {
                        sender.SendInfo($"Grupa {group.GetColoredName()}, nie posiada tyle pieniędzy na koncie.");
                    }
                }
                else
                {
                    sender.SendWarning("Nie posiadasz uprawnień do wypłacania gotówki.");
                }
            }
            else
            {
                sender.SendInfo("Nie posiadasz grupy w tym slocie.");
            }
        }

        [Command("gwplac")]
        public void PutMoneyIntoGroup(Client sender, short groupSlot, decimal safeMoneyCount)
        {
            MoneyValidator validator = new MoneyValidator();
            if (!validator.IsValid(safeMoneyCount))
            {
                sender.SendError("Podano kwotę gotówki w nieprawidłowym formacie.");
            }

            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group, out WorkerModel worker))
            {
                CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                if (character.HasMoney(safeMoneyCount))
                {
                    character.RemoveMoney(safeMoneyCount);
                    group.AddMoney(safeMoneyCount);

                    sender.SendInfo($"Wpłacono ${safeMoneyCount} na konto grupy {group.GetColoredName()}.");
                }
                else
                {
                    sender.SendError("Nie posiadasz tyle gotówki.");
                }
            }
            else
            {
                sender.SendInfo("Nie posiadasz grupy w tym slocie.");
            }
        }

        [Command("g")]
        public void ShowGroupMenu(Client sender, byte slot)
        {
            AccountEntity player = sender.GetAccountEntity();
            if (!EntityHelper.GetPlayerGroups(player).Any())
            {
                sender.SendWarning("Nie jesteś członkiem żadnej grupy.");
                return;
            }

            GroupSlotValidator validator = new GroupSlotValidator();
            if (!validator.IsValid(slot))
            {
                sender.SendError("Podano dane w nieprawidłowym formacie.");
                return;
            }

            if (sender.TryGetGroupByUnsafeSlot(slot, out GroupEntity group, out WorkerModel worker))
            {
                sender.TriggerEvent("ShowGroupMenu", JsonConvert.SerializeObject(new
                {
                    group.DbModel.Name,
                    group.DbModel.Tag,
                    group.DbModel.Money,
                    Color = group.DbModel.Color,
                    // To jest raczej kosztowne, ale nie widzę innej opcji
                    PlayersOnLine = JsonConvert.SerializeObject(group.GetWorkers().Where(x => x.Character.Online).Select(w => new
                    {
                        ServerId = EntityHelper.GetAccountByCharacterId(w.Character.Id).ServerId,
                        Name = $"{w.Character.Name} {w.Character.Surname}",
                        Salary = w.Salary,
                        DutyTime = w.DutyMinutes,
                        OnDuty = group.PlayersOnDuty.Contains(EntityHelper.GetAccountByCharacterId(w.Character.Id))
                    }))
                }), group.CanPlayerManageWorkers(worker));
            }
            else
            {
                sender.SendInfo("Nie posiadasz grupy w tym slocie.");
            }
        }

        [Command("gzapros")]
        public void InvitePlayerToGroup(Client sender, short groupSlot, int id)
        {
            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group, out WorkerModel worker))
            {
                if (group.CanPlayerManageWorkers(worker))
                {
                    AccountEntity getter = EntityHelper.GetAccountByServerId(id);
                    if (getter == null)
                    {
                        sender.SendError("Nie znaleziono gracza o podanym Id.");
                        return;
                    }

                    if (group.ContainsWorker(getter))
                    {
                        sender.SendInfo($"{getter.CharacterEntity.FormatName} już znajduje się w grupie {group.GetColoredName()}");
                        return;
                    }
                    group.AddWorker(getter);
                    getter.CharacterEntity.SendInfo($"Zostałeś zaproszony do grupy {group.GetColoredName()} przez {sender.Name}.");
                    sender.SendInfo($"Zaprosiłeś gracza {getter.Client.Name} do grupy {group.GetColoredName()}.");
                }
                else
                {
                    sender.SendWarning("Nie posiadasz uprawnień do zarządzania pracownikami.");
                }
            }
            else
            {
                sender.SendInfo("Nie posiadasz grupy w tym slocie.");
            }
        }

        [Command("gwypros")]
        public void RemovePlayerFromGroup(Client sender, short groupSlot, int id)
        {
            if (sender.TryGetGroupByUnsafeSlot(groupSlot, out GroupEntity group, out WorkerModel worker))
            {
                if (group.CanPlayerManageWorkers(worker))
                {
                    AccountEntity getter = EntityHelper.GetAccountByServerId(id);
                    if (getter == null)
                    {
                        sender.SendError("Nie znaleziono gracza o podanym Id.");
                        return;
                    }

                    if (group.ContainsWorker(getter))
                    {
                        sender.SendInfo($"{getter.CharacterEntity.FormatName} nie należy do grupy {group.GetColoredName()}");
                        return;
                    }

                    group.RemoveWorker(getter);
                    getter.CharacterEntity.SendInfo($"Zostałeś wyproszony z grupy {group.GetColoredName()} przez {sender.Name}.");
                    sender.SendInfo($"Wyprosiłeś gracza {getter.Client.Name} z grupy {group.GetColoredName()}.");
                }
                else
                {
                    sender.SendWarning("Nie posiadasz uprawnień do zarządzania pracownikami.");
                }
            }
            else
            {
                sender.SendInfo("Nie posiadasz grupy w tym slocie.");
            }
        }

        #endregion
    }
}