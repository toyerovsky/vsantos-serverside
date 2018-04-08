/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Core.Group
{
    public abstract class GroupEntity : IDbEntity<GroupModel>
    {
        public GroupModel DbModel { get; set; }

        public long Id => DbModel.Id;

        public List<AccountEntity> PlayersOnDuty { get; } = new List<AccountEntity>();

        protected GroupEntity(GroupModel dbModel)
        {
            DbModel = dbModel;
            EntityHelper.Add(this);
        }

        /// <summary>
        /// Dodawanie nowej grupy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static GroupEntity Create(string name, string tag, GroupType type, Color color)
        {
            GroupModel groupModel = new GroupModel
            {
                Workers = new List<WorkerModel>(),
                Name = name,
                Tag = tag,
                GroupType = type,
                Color = color.ToHex()
            };

            if (groupModel.GroupType == GroupType.Crime)
            {
                CrimeBotModel crimeBotModel = new CrimeBotModel
                {
                    Name = "",
                    GroupModel = groupModel
                };

                using (CrimeBotsRepository repository = new CrimeBotsRepository())
                {
                    repository.Insert(crimeBotModel);
                    repository.Save();
                }
            }

            using (GroupsRepository repository = new GroupsRepository())
            {
                repository.Insert(groupModel);
                repository.Save();
            }

            GroupEntityFactory factory = new GroupEntityFactory();
            return factory.Create(groupModel);
        }

        public string GetColoredName() => $"<p style='color:{DbModel.Color}'>{DbModel.Name}</p>";

        public bool HasMoney(decimal money) => DbModel.Money >= money;

        public void AddMoney(decimal money)
        {
            DbModel.Money += money;
            Save();
        }

        public void RemoveMoney(decimal money)
        {
            DbModel.Money -= money;
            Save();
        }

        public void AddWorker(AccountEntity account)
        {
            DbModel.Workers.Add(new WorkerModel
            {
                Group = DbModel,
                Character = account.CharacterEntity.DbModel,
                DutyMinutes = 0,
                ChatRight = false,
                DoorsRight = false,
                OfferFromWarehouseRight = false,
                PaycheckRight = false,
                RecrutationRight = false,
                Salary = 0
            });
            Save();
        }

        public void RemoveWorker(AccountEntity account)
        {
            DbModel.Workers.Remove(DbModel.Workers.Single(
                w => w.Character.Id == account.CharacterEntity.DbModel.Id));
            Save();
        }

        public List<WorkerModel> GetWorkers()
        {
            return DbModel.Workers.Where(w => w.Character != null).ToList();
        }

        public bool CanPlayerOffer(AccountEntity account)
        {
            return DbModel.Workers.Single(w => w.Character == account.CharacterEntity.DbModel).OfferFromWarehouseRight;
        }

        /// <summary>
        /// Czy gracz może zapraszać i wypraszać ludzi z grupy
        /// </summary>
        /// <param Name="account"></param>
        /// <returns></returns>
        public bool CanPlayerManageWorkers(AccountEntity account)
        {
            return DbModel.Workers.Single(w => w.Character == account.CharacterEntity.DbModel).RecrutationRight;
        }

        public bool CanPlayerTakeMoney(AccountEntity account)
        {
            return DbModel.Workers.Single(w => w.Character == account.CharacterEntity.DbModel).PaycheckRight;
        }

        public bool CanPlayerWriteOnChat(AccountEntity account)
        {
            return DbModel.Workers.Single(w => w.Character == account.CharacterEntity.DbModel).ChatRight;
        }

        public bool CanPlayerBuyInWarehouse(AccountEntity account)
        {
            return DbModel.Workers.Single(w => w.Character == account.CharacterEntity.DbModel).OrderFromWarehouseRight;
        }

        public bool ContainsWorker(AccountEntity account)
        {
            return DbModel.Workers.Any(w => w.Character.Id == account.CharacterEntity.DbModel.Id);
        }

        public void Save()
        {
            using (GroupsRepository repository = new GroupsRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        public void Dispose()
        {
        }

        public override string ToString() => GetColoredName();
    }
}