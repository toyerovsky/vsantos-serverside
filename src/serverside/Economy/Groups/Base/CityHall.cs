/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using VRP.Core.Database.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;

namespace VRP.Serverside.Economy.Groups.Base
{
    public class CityHall : GroupEntity
    {
        /* OPCJONALNE PRAWA
         * 1 - wydawanie dowodu osobistego 
         * 2 - wydawanie prawa jazdy
         */

        public CityHall(GroupModel editor) : base(editor)
        {
        }

        public bool CanPlayerGiveIdCard(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.FirstRight.HasValue && workerModel.FirstRight.Value;
        }

        public bool CanPlayerGiveDrivingLicense(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.SecondRight.HasValue && workerModel.SecondRight.Value;
        }
    }
}