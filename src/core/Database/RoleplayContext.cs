﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Character;
using VRP.Core.Database.Models.Lease;
using VRP.Core.Database.Models.Mdt;
using VRP.Core.Database.Models.Misc;
using VRP.Core.Database.Models.Vehicle;

namespace VRP.Core.Database
{
    public class RoleplayContext : DbContext
    {
        public RoleplayContext(DbContextOptions options) : base(options)
        {
        }

        #region Account
        public virtual DbSet<AccountModel> Accounts { get; set; }
        public virtual DbSet<PenaltyModel> Penaltlies { get; set; }
        #endregion

        #region Agreement
        public virtual DbSet<AgreementModel> Agreements { get; set; }
        public virtual DbSet<LeaseModel> Leases { get; set; }
        #endregion

        #region Building
        public virtual DbSet<BuildingModel> Buildings { get; set; }
        #endregion

        #region Character
        public virtual DbSet<CharacterModel> Characters { get; set; }
        public virtual DbSet<CharacterLookModel> CharacterLooks { get; set; }
        #endregion

        #region CrimeBot
        public virtual DbSet<CrimeBotModel> CrimeBots { get; set; }
        public virtual DbSet<CrimeBotItemModel> CrimeBotItems { get; set; }
        #endregion

        #region Groups
        public virtual DbSet<GroupModel> Groups { get; set; }
        public virtual DbSet<WorkerModel> Workers { get; set; }
        #endregion

        #region Item
        public virtual DbSet<ItemModel> Items { get; set; }
        public virtual DbSet<ItemTemplateModel> ItemTemplates { get; set; }
        #endregion

        #region Mdt
        public virtual DbSet<CriminalCaseModel> CriminalCases { get; set; }
        public virtual DbSet<CriminalCaseVehicleRecordRelation> CriminalCaseVehicleRecordRelations { get; set; }
        public virtual DbSet<CriminalCaseCharacterRecordRelation> CriminalCaseCharacterRecordRelations { get; set; }
        public virtual DbSet<CharacterRecordModel> CharacterRecords { get; set; }
        public virtual DbSet<VehicleRecordModel> VehicleRecordModels { get; set; }
        #endregion

        #region Misc
        public virtual DbSet<DescriptionModel> Descriptions { get; set; }
        public virtual DbSet<ZoneModel> Zones { get; set; }
        public virtual DbSet<AutoSaleModel> AutoSales { get; set; }
        #endregion

        #region Telephone
        public virtual DbSet<TelephoneContactModel> TelephoneContacts { get; set; }
        public virtual DbSet<TelephoneMessageModel> TelephoneMessages { get; set; }
        #endregion

        #region Vehicle
        public virtual DbSet<VehicleModel> Vehicles { get; set; }
        public virtual DbSet<VehicleTuningModel> VehicleTunings { get; set; }
        #endregion

        #region Warehouse 
        public virtual DbSet<GroupWarehouseItemModel> GroupWarehouseItems { get; set; }
        public virtual DbSet<GroupWarehouseOrderModel> GroupWarehouseOrders { get; set; }
        public virtual DbSet<GroupWarehouseModel> GroupWarehouses { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterModel>()
                .HasOne(character => character.CharacterLookModel)
                .WithOne(characterLook => characterLook.CharacterModel)
                .HasForeignKey<CharacterModel>();

            modelBuilder.Entity<BuildingModel>()
                .HasOne(building => building.AutoSaleModel)
                .WithOne(buildingForSale => buildingForSale.BuildingModel)
                .HasForeignKey<BuildingModel>();

            modelBuilder.Entity<AgreementModel>()
                .HasOne(agreement => agreement.LeaseModel)
                .WithOne(lease => lease.AgreementModel)
                .HasForeignKey<AgreementModel>();
        }
    }
}

