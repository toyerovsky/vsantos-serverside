/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Jobs.Enums;

namespace Serverside.Employer
{
    public class EmployerScript : Script
    {
        private FullPosition EmployerPosition => new FullPosition(new Vector3(1750f, -1580f, 113f), new Vector3(1f, 1f, 1f));
        private EmployerBot Employer { get; }

        public EmployerScript()
        {
            Event.OnResourceStart += OnResourceStart;
            Employer = new EmployerBot(Event, "John Smith", PedHash.Business01AMM, EmployerPosition);
            Employer.Intialize();
        }

        public void OnResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(EmployerScript)}] {Constant.ConstantMessages.ResourceStartMessage}!", ConsoleColor.DarkMagenta);
        }

        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            //args[0] to numerek pracy
            //0 Śmieciarz
            //1 Greenkeeper
            //2 Złodziej
            //3 Magazynier
            if (eventName == "OnPlayerSelectedJob")
            {
                var player = sender.GetAccountEntity();
                player.CharacterEntity.DbModel.Job = (JobType)Convert.ToInt32(arguments[0]);
                player.Save();

                switch (player.CharacterEntity.DbModel.Job)
                {
                    case JobType.Dustman:
                        sender.Notify("Podjąłeś się pracy: Operator śmieciarki. Udaj się na wysypisko i wsiądź do śmieciarki.");
                        break;
                    case JobType.Greenkeeper:
                        sender.Notify("Podjąłeś się pracy: Greenkeeper. Udaj się na pole golfowe i wsiądź do kosiarki.");
                        break;
                    case JobType.Thief:
                        sender.Notify("Podjąłeś się pracy: Złodziej. Udaj się do portu i wsiądź do jednej z ciężarówek.");
                        break;
                    case JobType.Courier:
                        sender.Notify("Podjąłeś się pracy: Kurier. Udaj się do magazynu, jest on oznaczony na mAPIe ikoną TU WPISAC.");
                        break;
                }
            }
            else if (eventName == "OnPlayerTakeMoneyJob")
            {
                var player = sender.GetAccountEntity();
                if (player.CharacterEntity.DbModel.MoneyJob != null)
                {
                    sender.AddMoney((decimal)player.CharacterEntity.DbModel.MoneyJob);
                    player.CharacterEntity.DbModel.MoneyJob = 0;
                    player.CharacterEntity.Save();
                }
            }
        }
    }
}