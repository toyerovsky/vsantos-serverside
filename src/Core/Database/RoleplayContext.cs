/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database.Models;

namespace Serverside.Core.Database
{
    public class RoleplayContext : DbContext
    {
        public RoleplayContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<AccountModel> Accounts { get; set; }
        public virtual DbSet<PenaltyModel> Penaltlies { get; set; }
        public virtual DbSet<CharacterModel> Characters { get; set; }
        public virtual DbSet<VehicleModel> Vehicles { get; set; }
        public virtual DbSet<GroupModel> Groups { get; set; }
        public virtual DbSet<ItemModel> Items { get; set; }
        public virtual DbSet<BuildingModel> Buildings { get; set; }
        public virtual DbSet<CrimeBotModel> CrimeBots { get; set; }
        public virtual DbSet<DescriptionModel> Descriptions { get; set; }
        public virtual DbSet<TelephoneContactModel> TelephoneContacts { get; set; }
        public virtual DbSet<TelephoneMessageModel> TelephoneMessages { get; set; }
        public virtual DbSet<WorkerModel> Workers { get; set; }
        public virtual DbSet<GroupWarehouseItemModel> GroupWarehouseItems { get; set; }
        public virtual DbSet<GroupWarehouseOrderModel> GroupWarehouseOrders { get; set; }
    }
}

