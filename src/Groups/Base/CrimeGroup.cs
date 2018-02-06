/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Groups.Base
{
    public class CrimeGroup : GroupEntity
    {
        /* OPCJONALNE PRAWA
         * 1 - wzywanie Crime Bota
         */
        public CrimeBot.CrimeBot CrimeBot { get; set; }

        public CrimeGroup(GroupModel editor) : base(editor)
        {
        }

        public bool CanPlayerCallCrimeBot(AccountEntity account)
        {
            if (!ContainsWorker(account)) return false;
            WorkerModel workerModel = DbModel.Workers.First(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
            return workerModel.FirstRight.HasValue && workerModel.FirstRight.Value;
        }

    }
}