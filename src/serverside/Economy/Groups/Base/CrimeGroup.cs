/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Linq;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Group;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Group;
using VRP.Serverside.Entities.Peds.CrimeBot;

namespace VRP.Serverside.Economy.Groups.Base
{
    public class CrimeGroup : GroupEntity
    {
        /* OPCJONALNE PRAWA
         * 1 - wzywanie Crime Bota
         */
        public CrimePedEntity CrimePedEntity { get; set; }

        public CrimeGroup(GroupModel editor) : base(editor)
        {
        }

        public bool CanPlayerCallCrimeBot(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.Rights.HasFlag(GroupRights.First);
        }

    }
}