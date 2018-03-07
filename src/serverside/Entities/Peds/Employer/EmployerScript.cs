/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Peds.Employer
{
    public class EmployerScript : Script
    {
        private FullPosition EmployerPosition => new FullPosition(new Vector3(1750f, -1580f, 113f), new Vector3(1f, 1f, 1f));
        private EmployerPedEntity Employer { get; set; }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            //FixMe na razie nie ma wsparcia dla przechodniów po stronie serwera
            //Employer = new EmployerPedEntity(Event, "John Smith", PedHash.Business01AMM, EmployerPosition);
            //Employer.Spawn();
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
                AccountEntity player = sender.GetAccountEntity();
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
                        sender.Notify("Podjąłeś się pracy: Kurier. Udaj się do magazynu, jest on oznaczony na mapie ikoną FixMe.");
                        break;
                }
            }
            else if (eventName == "OnPlayerTakeMoneyJob")
            {
                AccountEntity player = sender.GetAccountEntity();
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