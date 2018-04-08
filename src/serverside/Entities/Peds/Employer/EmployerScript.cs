/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
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
                        sender.SendInfo("Podjąłeś się pracy: Operator śmieciarki. Udaj się na wysypisko i wsiądź do śmieciarki.");
                        break;
                    case JobType.Greenkeeper:
                        sender.SendInfo("Podjąłeś się pracy: Ogrodnik. Udaj się na pole golfowe i wsiądź do kosiarki.");
                        break;
                    case JobType.Thief:
                        sender.SendInfo("Podjąłeś się pracy: Złodziej. Udaj się do portu i wsiądź do jednej z ciężarówek.");
                        break;
                    case JobType.Courier:
                        sender.SendInfo("Podjąłeś się pracy: Kurier. Udaj się do magazynu, jest on oznaczony na mapie ikoną FixMe.");
                        break;
                }
            }
            else if (eventName == "OnPlayerTakeMoneyJob")
            {
                CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                if (character.DbModel.MoneyJob != null)
                {
                    character.AddMoney(character.DbModel.MoneyJob.Value);
                    character.DbModel.MoneyJob = 0;
                    character.Save();
                }
            }
        }
    }
}