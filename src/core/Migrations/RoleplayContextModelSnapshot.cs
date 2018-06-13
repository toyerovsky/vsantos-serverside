﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VRP.Core.Database;
using VRP.Core.Enums;

namespace VRP.Core.Migrations
{
    [DbContext(typeof(RoleplayContext))]
    partial class RoleplayContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("VRP.Core.Database.Models.Account.AccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<long>("ForumUserId");

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<long>("PrimaryForumGroup");

                    b.Property<string>("SecondaryForumGroups");

                    b.Property<string>("SerialsJson");

                    b.Property<int>("ServerRank");

                    b.Property<string>("SocialClub")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Account.PenaltyModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountId");

                    b.Property<int?>("CreatorId");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<int>("PenaltyType");

                    b.Property<string>("Reason")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Penaltlies");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Agreement.AgreementModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AutoRenewal");

                    b.Property<DateTime>("End");

                    b.Property<int?>("LeaserCharacterId");

                    b.Property<int?>("LeaserGroupId");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.HasIndex("LeaserCharacterId");

                    b.HasIndex("LeaserGroupId");

                    b.ToTable("Agreements");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Agreement.LeaseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgreementId");

                    b.Property<int?>("BuildingId");

                    b.Property<TimeSpan>("ChargeFrequency");

                    b.Property<decimal>("Cost");

                    b.Property<int?>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("AgreementId")
                        .IsUnique();

                    b.HasIndex("BuildingId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Leases");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Building.BuildingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AutoSaleId");

                    b.Property<int?>("CharacterId");

                    b.Property<int?>("CreatorId");

                    b.Property<short>("CurrentObjectCount");

                    b.Property<string>("Description");

                    b.Property<decimal?>("EnterCharge");

                    b.Property<float>("ExternalPickupPositionX");

                    b.Property<float>("ExternalPickupPositionY");

                    b.Property<float>("ExternalPickupPositionZ");

                    b.Property<int?>("GroupId");

                    b.Property<bool>("HasCctv");

                    b.Property<bool>("HasSafe");

                    b.Property<uint>("InternalDimension");

                    b.Property<float>("InternalPickupPositionX");

                    b.Property<float>("InternalPickupPositionY");

                    b.Property<float>("InternalPickupPositionZ");

                    b.Property<short>("MaxObjectCount");

                    b.Property<string>("Name");

                    b.Property<bool>("SpawnPossible");

                    b.HasKey("Id");

                    b.HasIndex("AutoSaleId")
                        .IsUnique();

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Character.CharacterLookModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("AccessoryId");

                    b.Property<byte?>("AccessoryTexture");

                    b.Property<int>("CharacterId");

                    b.Property<byte?>("EarsId");

                    b.Property<byte?>("EarsTexture");

                    b.Property<float?>("EyeBrowsOpacity");

                    b.Property<byte?>("EyebrowsId");

                    b.Property<byte?>("FatherId");

                    b.Property<byte?>("FirstEyebrowsColor");

                    b.Property<byte?>("FirstLipstickColor");

                    b.Property<byte?>("FirstMakeupColor");

                    b.Property<byte?>("GlassesId");

                    b.Property<byte?>("GlassesTexture");

                    b.Property<byte?>("HairColor");

                    b.Property<byte?>("HairId");

                    b.Property<byte?>("HairTexture");

                    b.Property<byte?>("HatId");

                    b.Property<byte?>("HatTexture");

                    b.Property<byte?>("LegsId");

                    b.Property<byte?>("LegsTexture");

                    b.Property<float?>("LipstickOpacity");

                    b.Property<byte?>("MakeupId");

                    b.Property<float?>("MakeupOpacity");

                    b.Property<byte?>("MotherId");

                    b.Property<byte?>("SecondEyebrowsColor");

                    b.Property<byte?>("SecondLipstickColor");

                    b.Property<byte?>("SecondMakeupColor");

                    b.Property<float?>("ShapeMix");

                    b.Property<byte?>("ShoesId");

                    b.Property<byte?>("ShoesTexture");

                    b.Property<byte?>("TopId");

                    b.Property<byte?>("TopTexture");

                    b.Property<byte?>("TorsoId");

                    b.Property<byte?>("UndershirtId");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.ToTable("CharacterLooks");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Character.CharacterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<int?>("BankAccountNumber");

                    b.Property<decimal>("BankMoney");

                    b.Property<DateTime?>("BornDate");

                    b.Property<int>("CharacterLookId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<uint>("CurrentDimension");

                    b.Property<bool>("Gender");

                    b.Property<bool>("HasDrivingLicense");

                    b.Property<bool>("HasIdCard");

                    b.Property<byte>("Health");

                    b.Property<bool>("IsAlive");

                    b.Property<DateTime>("LastLoginTime");

                    b.Property<float>("LastPositionX");

                    b.Property<float>("LastPositionY");

                    b.Property<float>("LastPositionZ");

                    b.Property<float>("LastRotationX");

                    b.Property<float>("LastRotationY");

                    b.Property<float>("LastRotationZ");

                    b.Property<int>("MinutesToRespawn");

                    b.Property<string>("Model");

                    b.Property<decimal>("Money");

                    b.Property<string>("Name");

                    b.Property<bool>("Online");

                    b.Property<int>("PartTimeJobWorkerId");

                    b.Property<TimeSpan>("PlayedTime");

                    b.Property<string>("Surname");

                    b.Property<TimeSpan>("TodayPlayedTime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBot.CrimeBotItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<int>("Count");

                    b.Property<int?>("CrimeBotModelId");

                    b.Property<int?>("ItemTemplateModelId");

                    b.Property<int>("ResetCount");

                    b.HasKey("Id");

                    b.HasIndex("CrimeBotModelId");

                    b.HasIndex("ItemTemplateModelId");

                    b.ToTable("CrimeBotItems");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBot.CrimeBotModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatorId");

                    b.Property<int?>("GroupModelId");

                    b.Property<string>("Name");

                    b.Property<string>("PedSkin");

                    b.Property<string>("VehicleModel");

                    b.HasKey("Id");

                    b.HasIndex("GroupModelId");

                    b.ToTable("CrimeBots");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Group.GroupModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BossCharacterId");

                    b.Property<string>("Color");

                    b.Property<int>("Dotation");

                    b.Property<int>("GroupType");

                    b.Property<int>("MaxPayday");

                    b.Property<decimal>("Money");

                    b.Property<string>("Name");

                    b.Property<string>("Tag");

                    b.HasKey("Id");

                    b.HasIndex("BossCharacterId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Group.WorkerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<int>("DutyMinutes");

                    b.Property<int?>("GroupId");

                    b.Property<int>("Rights");

                    b.Property<decimal>("Salary");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Item.ItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuildingId");

                    b.Property<int?>("CharacterId");

                    b.Property<int?>("CreatorId");

                    b.Property<int?>("FirstParameter");

                    b.Property<int?>("FourthParameter");

                    b.Property<int>("ItemEntityType");

                    b.Property<string>("ItemHash");

                    b.Property<string>("Name");

                    b.Property<int?>("OwnerVehicleId");

                    b.Property<int?>("SecondParameter");

                    b.Property<int?>("ThirdParameter");

                    b.Property<int?>("TuningInVehicleId");

                    b.Property<short>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("CharacterId");

                    b.HasIndex("OwnerVehicleId");

                    b.HasIndex("TuningInVehicleId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Item.ItemTemplateModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FirstParameter");

                    b.Property<int?>("FourthParameter");

                    b.Property<int>("ItemEntityType");

                    b.Property<string>("ItemHash");

                    b.Property<string>("Name");

                    b.Property<int?>("SecondParameter");

                    b.Property<int?>("ThirdParameter");

                    b.Property<short>("Weight");

                    b.HasKey("Id");

                    b.ToTable("ItemTemplates");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CharacterRecordModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("BornDate");

                    b.Property<Guid>("DNACode");

                    b.Property<char>("EyeColor");

                    b.Property<string>("FingerPrints");

                    b.Property<bool>("Gender");

                    b.Property<byte[]>("Image");

                    b.Property<string>("Name");

                    b.Property<int>("Race");

                    b.Property<string>("Surname");

                    b.Property<bool>("Wanted");

                    b.HasKey("Id");

                    b.ToTable("CharacterRecords");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CriminalCaseCharacterRecordRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterRecordId");

                    b.Property<int?>("CriminalCaseId");

                    b.HasKey("Id");

                    b.HasIndex("CharacterRecordId");

                    b.HasIndex("CriminalCaseId");

                    b.ToTable("CriminalCaseCharacterRecordRelations");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CriminalCaseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("CriminalCases");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CriminalCaseVehicleRecordRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CriminalCaseId");

                    b.Property<int?>("VehicleRecordId");

                    b.HasKey("Id");

                    b.HasIndex("CriminalCaseId");

                    b.HasIndex("VehicleRecordId");

                    b.ToTable("CriminalCaseVehicleRecordRelations");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.VehicleRecordModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<byte[]>("Image");

                    b.Property<string>("Model");

                    b.Property<string>("NumberPlate");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("SpecialFeatures");

                    b.Property<bool>("Towed");

                    b.Property<bool>("Wanted");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("VehicleRecordModels");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Misc.AutoSaleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.HasKey("Id");

                    b.ToTable("AutoSales");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Misc.DescriptionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.Property<int?>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Misc.ZoneModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatorId");

                    b.Property<string>("Name");

                    b.Property<string>("ZonePropertiesJson");

                    b.Property<int>("ZoneType");

                    b.HasKey("Id");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Telephone.TelephoneContactModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CellphoneId");

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.HasIndex("CellphoneId");

                    b.ToTable("TelephoneContacts");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Telephone.TelephoneMessageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CellphoneId");

                    b.Property<string>("Content")
                        .HasMaxLength(256);

                    b.Property<int>("SenderNumber");

                    b.HasKey("Id");

                    b.HasIndex("CellphoneId");

                    b.ToTable("TelephoneMessages");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Vehicle.VehicleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AutoSaleId");

                    b.Property<int?>("CharacterId");

                    b.Property<int?>("CreatorId");

                    b.Property<bool>("Door1Damage");

                    b.Property<bool>("Door2Damage");

                    b.Property<bool>("Door3Damage");

                    b.Property<bool>("Door4Damage");

                    b.Property<float>("EnginePowerMultiplier");

                    b.Property<float>("EngineTorqueMultiplier");

                    b.Property<float>("Fuel");

                    b.Property<float>("FuelConsumption");

                    b.Property<float>("FuelTank");

                    b.Property<int?>("GroupId");

                    b.Property<float>("Health");

                    b.Property<bool>("IsSpawned");

                    b.Property<float>("Milage");

                    b.Property<string>("Name");

                    b.Property<string>("NumberPlate");

                    b.Property<int>("NumberPlateStyle");

                    b.Property<int?>("PartTimeJobModelId");

                    b.Property<string>("PrimaryColor");

                    b.Property<string>("SecondaryColor");

                    b.Property<float>("SpawnPositionX");

                    b.Property<float>("SpawnPositionY");

                    b.Property<float>("SpawnPositionZ");

                    b.Property<float>("SpawnRotationX");

                    b.Property<float>("SpawnRotationY");

                    b.Property<float>("SpawnRotationZ");

                    b.Property<string>("VehicleHash");

                    b.Property<int>("WheelColor");

                    b.Property<int>("WheelType");

                    b.Property<bool>("Window1Damage");

                    b.Property<bool>("Window2Damage");

                    b.Property<bool>("Window3Damage");

                    b.Property<bool>("Window4Damage");

                    b.HasKey("Id");

                    b.HasIndex("AutoSaleId")
                        .IsUnique();

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PartTimeJobModelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<int>("Count");

                    b.Property<int>("GroupType");

                    b.Property<int?>("GroupWarehouseModelId");

                    b.Property<int?>("ItemTemplateModelId");

                    b.Property<int>("ResetCount");

                    b.HasKey("Id");

                    b.HasIndex("GroupWarehouseModelId");

                    b.HasIndex("ItemTemplateModelId");

                    b.ToTable("GroupWarehouseItems");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GroupId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupWarehouses");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseOrderModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GetterId");

                    b.Property<int>("OrderedItemCount");

                    b.Property<int?>("OrderedItemTemplateId");

                    b.Property<string>("ShipmentLog");

                    b.HasKey("Id");

                    b.HasIndex("GetterId");

                    b.HasIndex("OrderedItemTemplateId");

                    b.ToTable("GroupWarehouseOrders");
                });

            modelBuilder.Entity("VRP.Core.Database.PartTimeJob.PartTimeJobEmployerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("PartTimeJobId");

                    b.Property<int?>("PartTimeJobModelId");

                    b.Property<float>("PositionX");

                    b.Property<float>("PositionY");

                    b.Property<float>("PositionZ");

                    b.Property<float>("RotationX");

                    b.Property<float>("RotationY");

                    b.Property<float>("RotationZ");

                    b.HasKey("Id");

                    b.HasIndex("PartTimeJobModelId");

                    b.ToTable("PartTimeJobEmployerModel");
                });

            modelBuilder.Entity("VRP.Core.Database.PartTimeJob.PartTimeJobModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("DailyMoneyLimit");

                    b.Property<int>("JobType");

                    b.HasKey("Id");

                    b.ToTable("PartTimeJobModel");
                });

            modelBuilder.Entity("VRP.Core.Database.PartTimeJob.PartTimeJobWorkerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacterId");

                    b.Property<int>("PartTimeJobEmployerId");

                    b.Property<int?>("PartTimeJobEmployerModelId");

                    b.Property<decimal>("Salary");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.HasIndex("PartTimeJobEmployerModelId");

                    b.ToTable("PartTimeJobWorkerModel");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Account.PenaltyModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Account.AccountModel", "Account")
                        .WithMany("Penalties")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Agreement.AgreementModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "LeaserCharacter")
                        .WithMany("Agreements")
                        .HasForeignKey("LeaserCharacterId");

                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "LeaserGroup")
                        .WithMany("Agreements")
                        .HasForeignKey("LeaserGroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Agreement.LeaseModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Agreement.AgreementModel", "AgreementModel")
                        .WithOne("LeaseModel")
                        .HasForeignKey("VRP.Core.Database.Models.Agreement.LeaseModel", "AgreementId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VRP.Core.Database.Models.Building.BuildingModel", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId");

                    b.HasOne("VRP.Core.Database.Models.Vehicle.VehicleModel", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Building.BuildingModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Misc.AutoSaleModel", "AutoSaleModel")
                        .WithOne("BuildingModel")
                        .HasForeignKey("VRP.Core.Database.Models.Building.BuildingModel", "AutoSaleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithMany("Buildings")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Character.CharacterLookModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithOne("CharacterLook")
                        .HasForeignKey("VRP.Core.Database.Models.Character.CharacterLookModel", "CharacterId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Character.CharacterModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Account.AccountModel", "Account")
                        .WithMany("Characters")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBot.CrimeBotItemModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CrimeBot.CrimeBotModel")
                        .WithMany("CrimeBotItems")
                        .HasForeignKey("CrimeBotModelId");

                    b.HasOne("VRP.Core.Database.Models.Item.ItemTemplateModel", "ItemTemplateModel")
                        .WithMany()
                        .HasForeignKey("ItemTemplateModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBot.CrimeBotModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "GroupModel")
                        .WithMany()
                        .HasForeignKey("GroupModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Group.GroupModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "BossCharacter")
                        .WithMany()
                        .HasForeignKey("BossCharacterId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Group.WorkerModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithMany("Workers")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "Group")
                        .WithMany("Workers")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Item.ItemModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Building.BuildingModel", "Building")
                        .WithMany("ItemsInBuilding")
                        .HasForeignKey("BuildingId");

                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithMany("Items")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.Vehicle.VehicleModel", "OwnerVehicle")
                        .WithMany("ItemsInVehicle")
                        .HasForeignKey("OwnerVehicleId");

                    b.HasOne("VRP.Core.Database.Models.Vehicle.VehicleModel", "TuningInVehicle")
                        .WithMany("VehicleTuning")
                        .HasForeignKey("TuningInVehicleId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CriminalCaseCharacterRecordRelation", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Mdt.CharacterRecordModel", "CharacterRecord")
                        .WithMany("CriminalCases")
                        .HasForeignKey("CharacterRecordId");

                    b.HasOne("VRP.Core.Database.Models.Mdt.CriminalCaseModel", "CriminalCase")
                        .WithMany("InvolvedPeople")
                        .HasForeignKey("CriminalCaseId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.CriminalCaseVehicleRecordRelation", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Mdt.CriminalCaseModel", "CriminalCase")
                        .WithMany("InvolvedVehicles")
                        .HasForeignKey("CriminalCaseId");

                    b.HasOne("VRP.Core.Database.Models.Mdt.VehicleRecordModel", "VehicleRecord")
                        .WithMany("CriminalCases")
                        .HasForeignKey("VehicleRecordId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Mdt.VehicleRecordModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Mdt.CharacterRecordModel", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Misc.DescriptionModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithMany("Descriptions")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.Vehicle.VehicleModel", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Telephone.TelephoneContactModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Item.ItemModel", "Cellphone")
                        .WithMany()
                        .HasForeignKey("CellphoneId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Telephone.TelephoneMessageModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Item.ItemModel", "Cellphone")
                        .WithMany()
                        .HasForeignKey("CellphoneId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Vehicle.VehicleModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Misc.AutoSaleModel", "AutoSaleModel")
                        .WithOne("VehicleModel")
                        .HasForeignKey("VRP.Core.Database.Models.Vehicle.VehicleModel", "AutoSaleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithMany("Vehicles")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("VRP.Core.Database.PartTimeJob.PartTimeJobModel")
                        .WithMany("Vehicles")
                        .HasForeignKey("PartTimeJobModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseItemModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Warehouse.GroupWarehouseModel", "GroupWarehouseModel")
                        .WithMany("ItemsInWarehouse")
                        .HasForeignKey("GroupWarehouseModelId");

                    b.HasOne("VRP.Core.Database.Models.Item.ItemTemplateModel", "ItemTemplateModel")
                        .WithMany()
                        .HasForeignKey("ItemTemplateModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Group.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.Warehouse.GroupWarehouseOrderModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Warehouse.GroupWarehouseModel", "Getter")
                        .WithMany()
                        .HasForeignKey("GetterId");

                    b.HasOne("VRP.Core.Database.Models.Item.ItemTemplateModel", "OrderedItemTemplate")
                        .WithMany()
                        .HasForeignKey("OrderedItemTemplateId");
                });

            modelBuilder.Entity("VRP.Core.Database.PartTimeJob.PartTimeJobEmployerModel", b =>
                {
                    b.HasOne("VRP.Core.Database.PartTimeJob.PartTimeJobModel", "PartTimeJobModel")
                        .WithMany("Employers")
                        .HasForeignKey("PartTimeJobModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.PartTimeJob.PartTimeJobWorkerModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.Character.CharacterModel", "Character")
                        .WithOne("PartTimeJobWorkerModel")
                        .HasForeignKey("VRP.Core.Database.PartTimeJob.PartTimeJobWorkerModel", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VRP.Core.Database.PartTimeJob.PartTimeJobEmployerModel", "PartTimeJobEmployerModel")
                        .WithMany("Workers")
                        .HasForeignKey("PartTimeJobEmployerModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
