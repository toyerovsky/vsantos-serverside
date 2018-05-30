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

            modelBuilder.Entity("VRP.Core.Database.Models.AccountModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.BuildingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<decimal?>("Cost");

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

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CharacterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte?>("AccessoryId");

                    b.Property<byte?>("AccessoryTexture");

                    b.Property<int?>("AccountId");

                    b.Property<int?>("BankAccountNumber");

                    b.Property<decimal>("BankMoney");

                    b.Property<DateTime>("BornDate");

                    b.Property<DateTime>("CreateTime");

                    b.Property<uint>("CurrentDimension");

                    b.Property<byte?>("EarsId");

                    b.Property<byte?>("EarsTexture");

                    b.Property<float?>("EyeBrowsOpacity");

                    b.Property<byte?>("EyebrowsId");

                    b.Property<byte?>("FatherId");

                    b.Property<byte?>("FirstEyebrowsColor");

                    b.Property<byte?>("FirstLipstickColor");

                    b.Property<byte?>("FirstMakeupColor");

                    b.Property<string>("ForumDescription");

                    b.Property<bool>("Freemode");

                    b.Property<bool>("Gender");

                    b.Property<byte?>("GlassesId");

                    b.Property<byte?>("GlassesTexture");

                    b.Property<byte?>("HairColor");

                    b.Property<byte?>("HairId");

                    b.Property<byte?>("HairTexture");

                    b.Property<bool>("HasDrivingLicense");

                    b.Property<bool>("HasIdCard");

                    b.Property<byte?>("HatId");

                    b.Property<byte?>("HatTexture");

                    b.Property<byte>("Health");

                    b.Property<byte>("Height");

                    b.Property<bool>("IsAlive");

                    b.Property<bool?>("IsCreated");

                    b.Property<int>("Job");

                    b.Property<decimal?>("JobLimit");

                    b.Property<DateTime?>("JobReleaseDate");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<float>("LastPositionX");

                    b.Property<float>("LastPositionY");

                    b.Property<float>("LastPositionZ");

                    b.Property<float>("LastRotationX");

                    b.Property<float>("LastRotationY");

                    b.Property<float>("LastRotationZ");

                    b.Property<byte?>("LegsId");

                    b.Property<byte?>("LegsTexture");

                    b.Property<float?>("LipstickOpacity");

                    b.Property<byte?>("MakeupId");

                    b.Property<float?>("MakeupOpacity");

                    b.Property<int>("MinutesToRespawn");

                    b.Property<string>("Model");

                    b.Property<decimal>("Money");

                    b.Property<decimal?>("MoneyJob");

                    b.Property<byte?>("MotherId");

                    b.Property<string>("Name");

                    b.Property<bool>("Online");

                    b.Property<TimeSpan>("PlayedTime");

                    b.Property<byte?>("SecondEyebrowsColor");

                    b.Property<byte?>("SecondLipstickColor");

                    b.Property<byte?>("SecondMakeupColor");

                    b.Property<float?>("ShapeMix");

                    b.Property<byte?>("ShoesId");

                    b.Property<byte?>("ShoesTexture");

                    b.Property<string>("Story");

                    b.Property<string>("Surname");

                    b.Property<TimeSpan>("TodayPlayedTime");

                    b.Property<byte?>("TopId");

                    b.Property<byte?>("TopTexture");

                    b.Property<byte?>("TorsoId");

                    b.Property<byte?>("UndershirtId");

                    b.Property<byte>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBotModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("AmphetamineCost");

                    b.Property<int?>("AmphetamineCount");

                    b.Property<int?>("AmphetamineDefaultCount");

                    b.Property<decimal?>("AssaultRifleCost");

                    b.Property<int?>("AssaultRifleCount");

                    b.Property<int?>("AssaultRifleDefaultCount");

                    b.Property<decimal?>("AssaultRifleMagazineCost");

                    b.Property<int?>("AssaultRifleMagazineCount");

                    b.Property<int?>("AssaultRifleMagazineDefaultCount");

                    b.Property<decimal?>("AssaultRifleMk2Cost");

                    b.Property<int?>("AssaultRifleMk2Count");

                    b.Property<int?>("AssaultRifleMk2DefaultCount");

                    b.Property<decimal?>("AssaultRifleMk2MagazineCost");

                    b.Property<int?>("AssaultRifleMk2MagazineCount");

                    b.Property<int?>("AssaultRifleMk2MagazineDefaultCount");

                    b.Property<decimal?>("CocaineCost");

                    b.Property<int?>("CocaineCount");

                    b.Property<int?>("CocaineDefaultCount");

                    b.Property<decimal?>("CombatPistolCost");

                    b.Property<int?>("CombatPistolCount");

                    b.Property<int?>("CombatPistolDefaultCount");

                    b.Property<decimal?>("CombatPistolMagazineCost");

                    b.Property<int?>("CombatPistolMagazineCount");

                    b.Property<int?>("CombatPistolMagazineDefaultCount");

                    b.Property<decimal?>("CrackCost");

                    b.Property<int?>("CrackCount");

                    b.Property<int?>("CrackDefaultCount");

                    b.Property<int?>("CreatorId");

                    b.Property<decimal?>("DoubleBarrelShotgunCost");

                    b.Property<int?>("DoubleBarrelShotgunCount");

                    b.Property<int?>("DoubleBarrelShotgunDefaultCount");

                    b.Property<decimal?>("DoubleBarrelShotgunMagazineCost");

                    b.Property<int?>("DoubleBarrelShotgunMagazineCount");

                    b.Property<int?>("DoubleBarrelShotgunMagazineDefaultCount");

                    b.Property<decimal?>("ExcstasyCost");

                    b.Property<int?>("ExcstasyCount");

                    b.Property<int?>("ExcstasyDefaultCount");

                    b.Property<int?>("GroupModelId");

                    b.Property<decimal?>("HasishCost");

                    b.Property<int?>("HasishCount");

                    b.Property<int?>("HasishDefaultCount");

                    b.Property<decimal?>("HeavyPistolCost");

                    b.Property<int?>("HeavyPistolCount");

                    b.Property<int?>("HeavyPistolDefaultCount");

                    b.Property<decimal?>("HeavyPistolMagazineCost");

                    b.Property<int?>("HeavyPistolMagazineCount");

                    b.Property<int?>("HeavyPistolMagazineDefaultCount");

                    b.Property<decimal?>("HeroinCost");

                    b.Property<int?>("HeroinCount");

                    b.Property<int?>("HeroinDefaultCount");

                    b.Property<decimal?>("LsdCost");

                    b.Property<int?>("LsdCount");

                    b.Property<int?>("LsdDefaultCount");

                    b.Property<decimal?>("MarijuanaCost");

                    b.Property<int?>("MarijuanaCount");

                    b.Property<int?>("MarijuanaDefaultCount");

                    b.Property<decimal?>("MetaamphetamineCost");

                    b.Property<int?>("MetaamphetamineCount");

                    b.Property<int?>("MetaamphetamineDefaultCount");

                    b.Property<decimal?>("MicroSmgCost");

                    b.Property<int?>("MicroSmgCount");

                    b.Property<int?>("MicroSmgDefaultCount");

                    b.Property<decimal?>("MicroSmgMagazineCost");

                    b.Property<int?>("MicroSmgMagazineCount");

                    b.Property<int?>("MicroSmgMagazineDefaultCount");

                    b.Property<decimal?>("MiniSmgCost");

                    b.Property<int?>("MiniSmgCount");

                    b.Property<int?>("MiniSmgDefaultCount");

                    b.Property<decimal?>("MiniSmgMagazineCost");

                    b.Property<int?>("MiniSmgMagazineCount");

                    b.Property<int?>("MiniSmgMagazineDefaultCount");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.Property<decimal?>("Pistol50Cost");

                    b.Property<int?>("Pistol50Count");

                    b.Property<int?>("Pistol50DefaultCount");

                    b.Property<decimal?>("Pistol50MagazineCost");

                    b.Property<int?>("Pistol50MagazineCount");

                    b.Property<int?>("Pistol50MagazineDefaultCount");

                    b.Property<decimal?>("PistolCost");

                    b.Property<int?>("PistolCount");

                    b.Property<int?>("PistolDefaultCount");

                    b.Property<decimal?>("PistolMagazineCost");

                    b.Property<int?>("PistolMagazineCount");

                    b.Property<int?>("PistolMagazineDefaultCount");

                    b.Property<decimal?>("PistolMk2Cost");

                    b.Property<int?>("PistolMk2Count");

                    b.Property<int?>("PistolMk2DefaultCount");

                    b.Property<decimal?>("PistolMk2MagazineCost");

                    b.Property<int?>("PistolMk2MagazineCount");

                    b.Property<int?>("PistolMk2RMagazineDefaultCount");

                    b.Property<decimal?>("PumpShotgunCost");

                    b.Property<int?>("PumpShotgunCount");

                    b.Property<int?>("PumpShotgunDefaultCount");

                    b.Property<decimal?>("PumpShotgunMagazineCost");

                    b.Property<int?>("PumpShotgunMagazineCount");

                    b.Property<int?>("PumpShotgunMagazineDefaultCount");

                    b.Property<decimal?>("RevolverCost");

                    b.Property<int?>("RevolverCount");

                    b.Property<int?>("RevolverDefaultCount");

                    b.Property<decimal?>("RevolverMagazineCost");

                    b.Property<int?>("RevolverMagazineCount");

                    b.Property<int?>("RevolverMagazineDefaultCount");

                    b.Property<decimal?>("SawnoffShotgunCost");

                    b.Property<int?>("SawnoffShotgunCount");

                    b.Property<int?>("SawnoffShotgunDefaultCount");

                    b.Property<decimal?>("SawnoffShotgunMagazineCost");

                    b.Property<int?>("SawnoffShotgunMagazineCount");

                    b.Property<int?>("SawnoffShotgunMagazineDefaultCount");

                    b.Property<decimal?>("SmgCost");

                    b.Property<int?>("SmgCount");

                    b.Property<int?>("SmgDefaultCount");

                    b.Property<decimal?>("SmgMagazineCost");

                    b.Property<int?>("SmgMagazineCount");

                    b.Property<int?>("SmgMagazineDefaultCount");

                    b.Property<decimal?>("SmgMk2Cost");

                    b.Property<int?>("SmgMk2Count");

                    b.Property<int?>("SmgMk2DefaultCount");

                    b.Property<decimal?>("SmgMk2MagazineCost");

                    b.Property<int?>("SmgMk2MagazineCount");

                    b.Property<int?>("SmgMk2MagazineDefaultCount");

                    b.Property<decimal?>("SniperRifleCost");

                    b.Property<int?>("SniperRifleCount");

                    b.Property<int?>("SniperRifleDefaultCount");

                    b.Property<decimal?>("SniperRifleMagazineCost");

                    b.Property<int?>("SniperRifleMagazineCount");

                    b.Property<int?>("SniperRifleMagazineDefaultCount");

                    b.Property<decimal?>("SnsPistolCost");

                    b.Property<int?>("SnsPistolCount");

                    b.Property<int?>("SnsPistolDefaultCount");

                    b.Property<decimal?>("SnsPistolMagazineCost");

                    b.Property<int?>("SnsPistolMagazineCount");

                    b.Property<int?>("SnsPistolMagazineDefaultCount");

                    b.Property<string>("Vehicle");

                    b.HasKey("Id");

                    b.HasIndex("GroupModelId");

                    b.ToTable("CrimeBots");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.DescriptionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.GroupModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.GroupWarehouseItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<int>("Count");

                    b.Property<int>("GroupType");

                    b.Property<int?>("ItemModelId");

                    b.Property<decimal?>("MinimalCost");

                    b.Property<int>("ResetCount");

                    b.HasKey("Id");

                    b.HasIndex("ItemModelId");

                    b.ToTable("GroupWarehouseItems");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.GroupWarehouseOrderModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GetterId");

                    b.Property<string>("OrderItemsJson");

                    b.Property<string>("ShipmentLog");

                    b.HasKey("Id");

                    b.HasIndex("GetterId");

                    b.ToTable("GroupWarehouseOrders");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.ItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuildingId");

                    b.Property<int?>("CharacterId");

                    b.Property<int?>("CreatorId");

                    b.Property<int?>("FirstParameter");

                    b.Property<int?>("FourthParameter");

                    b.Property<int?>("GroupId");

                    b.Property<int>("ItemEntityType");

                    b.Property<string>("ItemHash");

                    b.Property<string>("Name");

                    b.Property<int?>("SecondParameter");

                    b.Property<int?>("ThirdParameter");

                    b.Property<int?>("VehicleId");

                    b.Property<short>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.PenaltyModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.TelephoneContactModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.TelephoneMessageModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.VehicleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.WorkerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<bool>("ChatRight");

                    b.Property<bool>("DoorsRight");

                    b.Property<int>("DutyMinutes");

                    b.Property<bool?>("EightRight");

                    b.Property<bool?>("FifthRight");

                    b.Property<bool?>("FirstRight");

                    b.Property<bool?>("FourthRight");

                    b.Property<int?>("GroupId");

                    b.Property<bool>("OfferFromWarehouseRight");

                    b.Property<bool>("OrderFromWarehouseRight");

                    b.Property<bool>("PaycheckRight");

                    b.Property<bool>("RecrutationRight");

                    b.Property<int>("Salary");

                    b.Property<bool?>("SecondRight");

                    b.Property<bool?>("SeventhRight");

                    b.Property<bool?>("SixthRight");

                    b.Property<bool?>("ThirdRight");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.ZoneModel", b =>
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

            modelBuilder.Entity("VRP.Core.Database.Models.BuildingModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "Character")
                        .WithMany("Buildings")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CharacterModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.AccountModel", "Account")
                        .WithMany("Characters")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.CrimeBotModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.GroupModel", "GroupModel")
                        .WithMany()
                        .HasForeignKey("GroupModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.DescriptionModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "Character")
                        .WithMany("Descriptions")
                        .HasForeignKey("CharacterId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.GroupModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "BossCharacter")
                        .WithMany()
                        .HasForeignKey("BossCharacterId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.GroupWarehouseItemModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.ItemModel", "ItemModel")
                        .WithMany()
                        .HasForeignKey("ItemModelId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.GroupWarehouseOrderModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.GroupModel", "Getter")
                        .WithMany()
                        .HasForeignKey("GetterId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.ItemModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.BuildingModel", "Building")
                        .WithMany("Items")
                        .HasForeignKey("BuildingId");

                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "Character")
                        .WithMany("Items")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("VRP.Core.Database.Models.VehicleModel", "Vehicle")
                        .WithMany("ItemsInVehicle")
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.PenaltyModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.AccountModel", "Account")
                        .WithMany("Penalties")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.TelephoneContactModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.ItemModel", "Cellphone")
                        .WithMany()
                        .HasForeignKey("CellphoneId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.TelephoneMessageModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.ItemModel", "Cellphone")
                        .WithMany()
                        .HasForeignKey("CellphoneId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.VehicleModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "Character")
                        .WithMany("Vehicles")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.GroupModel", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("VRP.Core.Database.Models.WorkerModel", b =>
                {
                    b.HasOne("VRP.Core.Database.Models.CharacterModel", "Character")
                        .WithMany("Workers")
                        .HasForeignKey("CharacterId");

                    b.HasOne("VRP.Core.Database.Models.GroupModel", "Group")
                        .WithMany("Workers")
                        .HasForeignKey("GroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
