/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Economy.Groups.Base
{
    public class Police : GroupEntity
    {
        /* OPCJONALNE PRAWA
         * 1 - Megafon
         * 2 - Kajdanki, Prowadzenie gracza, Wpychanie do pojazdu innego gracza
         * 3 - Blokady drogowe
         * 4 - Kolczatka
         */

        public Police(GroupModel editor) : base(editor)
        {
        }

        public bool CanPlayerUseMegaphone(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.FirstRight.HasValue && workerModel.FirstRight.Value;
        }

        public bool CanPlayerDoPolice(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.SecondRight.HasValue && workerModel.SecondRight.Value;
        }

        public bool CanPlayerPlaceRoadblocks(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.SecondRight.HasValue && workerModel.SecondRight.Value;
        }

        public bool CanPlayerPlaceSpike(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.SecondRight.HasValue && workerModel.SecondRight.Value;
        }
    }
}