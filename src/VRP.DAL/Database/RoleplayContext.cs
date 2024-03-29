﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Agreement;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Mdt;
using VRP.DAL.Database.Models.Misc;
using VRP.DAL.Database.Models.Telephone;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Database.Models.Warehouse;

namespace VRP.DAL.Database
{
    public class RoleplayContext : DbContext
    {
        private static readonly LoggerFactory RpLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public RoleplayContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(RpLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
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
        public virtual DbSet<GroupRankModel> GroupRanks { get; set; }
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
        #endregion

        #region Warehouse 
        public virtual DbSet<GroupWarehouseItemModel> GroupWarehouseItems { get; set; }
        public virtual DbSet<GroupWarehouseOrderModel> GroupWarehouseOrders { get; set; }
        public virtual DbSet<GroupWarehouseModel> GroupWarehouses { get; set; }
        #endregion

        #region Tickets
        public virtual DbSet<TicketModel> Tickets { get; set; }
        public virtual DbSet<TicketMessageModel> TicketMessages { get; set; }
        public virtual DbSet<TicketAdminRelation> TicketAdminRecordRelations { get; set; }
        public virtual DbSet<TicketUserRelation> TicketUserRecordRelations { get; set; }




        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterModel>()
                .HasOne(character => character.CharacterLook)
                .WithOne(characterLook => characterLook.Character)
                .HasForeignKey<CharacterLookModel>(characterLook => characterLook.CharacterId);

            #region AutoSaleModel

            modelBuilder.Entity<AutoSaleModel>()
                .HasOne(autoSale => autoSale.BuildingModel)
                .WithOne(building => building.AutoSaleModel)
                .HasForeignKey<BuildingModel>(building => building.AutoSaleId);

            modelBuilder.Entity<AutoSaleModel>()
                .HasOne(autoSale => autoSale.VehicleModel)
                .WithOne(vehicle => vehicle.AutoSaleModel)
                .HasForeignKey<VehicleModel>(vehicle => vehicle.AutoSaleId);

            #endregion

            modelBuilder.Entity<AgreementModel>()
                .HasOne(agreement => agreement.LeaseModel)
                .WithOne(lease => lease.Agreement)
                .HasForeignKey<LeaseModel>(lease => lease.AgreementId);

            modelBuilder.Entity<AccountModel>()
                .HasKey(account => account.Id);

            #region GroupModel

            modelBuilder.Entity<GroupModel>()
                .HasOne(group => group.DefaultRank)
                .WithOne(rank => rank.DefaultForGroup)
                .HasForeignKey<GroupRankModel>(groupRank => groupRank.DefaultForGroupId);

            modelBuilder.Entity<GroupModel>()
                .HasMany(group => group.GroupRanks)
                .WithOne(rank => rank.Group)
                .HasForeignKey(rank => rank.GroupId);

            #endregion
        }
    }
}

