/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Linq;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Enums;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;

namespace VRP.Serverside.Economy.Groups.Base
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
            return workerModel.Rights.HasFlag(GroupRights.First);
        }

        public bool CanPlayerDoPolice(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.Rights.HasFlag(GroupRights.Second);
        }

        public bool CanPlayerPlaceRoadblocks(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.Rights.HasFlag(GroupRights.Third);
        }

        public bool CanPlayerPlaceSpike(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.Rights.HasFlag(GroupRights.Fourth);
        }
    }
}