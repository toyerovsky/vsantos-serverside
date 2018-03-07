using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VRP.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    ForumGroup = table.Column<long>(nullable: false),
                    Ip = table.Column<string>(maxLength: 16, nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Online = table.Column<bool>(nullable: false),
                    OtherForumGroups = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    ServerRank = table.Column<int>(nullable: false),
                    SocialClub = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessoryId = table.Column<byte>(nullable: true),
                    AccessoryTexture = table.Column<byte>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    BankAccountNumber = table.Column<int>(nullable: true),
                    BankMoney = table.Column<decimal>(nullable: false),
                    BornDate = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CurrentDimension = table.Column<int>(nullable: false),
                    DivingEfficiency = table.Column<short>(nullable: false),
                    EarsId = table.Column<byte>(nullable: true),
                    EarsTexture = table.Column<byte>(nullable: true),
                    EyeBrowsOpacity = table.Column<float>(nullable: true),
                    EyebrowsId = table.Column<byte>(nullable: true),
                    FatherId = table.Column<byte>(nullable: true),
                    FirstEyebrowsColor = table.Column<byte>(nullable: true),
                    FirstLipstickColor = table.Column<byte>(nullable: true),
                    FirstMakeupColor = table.Column<byte>(nullable: true),
                    Force = table.Column<short>(nullable: false),
                    ForumDescription = table.Column<string>(nullable: true),
                    Freemode = table.Column<bool>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    GlassesId = table.Column<byte>(nullable: true),
                    GlassesTexture = table.Column<byte>(nullable: true),
                    HairColor = table.Column<byte>(nullable: true),
                    HairId = table.Column<byte>(nullable: true),
                    HairTexture = table.Column<byte>(nullable: true),
                    HasDrivingLicense = table.Column<bool>(nullable: false),
                    HasIdCard = table.Column<bool>(nullable: false),
                    HatId = table.Column<byte>(nullable: true),
                    HatTexture = table.Column<byte>(nullable: true),
                    Height = table.Column<short>(nullable: false),
                    HitPoints = table.Column<int>(nullable: false),
                    IsAlive = table.Column<bool>(nullable: false),
                    IsCreated = table.Column<bool>(nullable: true),
                    Job = table.Column<int>(nullable: false),
                    JobLimit = table.Column<decimal>(nullable: true),
                    JobReleaseDate = table.Column<DateTime>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    LastPositionRotX = table.Column<float>(nullable: false),
                    LastPositionRotY = table.Column<float>(nullable: false),
                    LastPositionRotZ = table.Column<float>(nullable: false),
                    LastPositionX = table.Column<float>(nullable: false),
                    LastPositionY = table.Column<float>(nullable: false),
                    LastPositionZ = table.Column<float>(nullable: false),
                    LegsId = table.Column<byte>(nullable: true),
                    LegsTexture = table.Column<byte>(nullable: true),
                    LipstickOpacity = table.Column<float>(nullable: true),
                    MakeupId = table.Column<byte>(nullable: true),
                    MakeupOpacity = table.Column<float>(nullable: true),
                    MinutesToRespawn = table.Column<int>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Money = table.Column<decimal>(nullable: false),
                    MoneyJob = table.Column<decimal>(nullable: true),
                    MotherId = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Online = table.Column<bool>(nullable: false),
                    PlayedTime = table.Column<DateTime>(nullable: true),
                    RunningEfficiency = table.Column<short>(nullable: false),
                    SecondEyebrowsColor = table.Column<byte>(nullable: true),
                    SecondLipstickColor = table.Column<byte>(nullable: true),
                    SecondMakeupColor = table.Column<byte>(nullable: true),
                    ShapeMix = table.Column<float>(nullable: true),
                    ShoesId = table.Column<byte>(nullable: true),
                    ShoesTexture = table.Column<byte>(nullable: true),
                    Story = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    TodayPlayedTime = table.Column<DateTime>(nullable: true),
                    TopId = table.Column<byte>(nullable: true),
                    TopTexture = table.Column<byte>(nullable: true),
                    TorsoId = table.Column<byte>(nullable: true),
                    UndershirtId = table.Column<byte>(nullable: true),
                    Weight = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Penaltlies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    PenaltyType = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penaltlies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penaltlies_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Penaltlies_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Descriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Descriptions_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BossCharacterId = table.Column<int>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Dotation = table.Column<int>(nullable: false),
                    GroupType = table.Column<int>(nullable: false),
                    MaxPayday = table.Column<int>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Characters_BossCharacterId",
                        column: x => x.BossCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: true),
                    Cost = table.Column<decimal>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CurrentObjectCount = table.Column<short>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EnterCharge = table.Column<decimal>(nullable: true),
                    ExternalPickupPositionX = table.Column<float>(nullable: false),
                    ExternalPickupPositionY = table.Column<float>(nullable: false),
                    ExternalPickupPositionZ = table.Column<float>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    HasCctv = table.Column<bool>(nullable: false),
                    HasSafe = table.Column<bool>(nullable: false),
                    InternalDimension = table.Column<uint>(nullable: false),
                    InternalPickupPositionX = table.Column<float>(nullable: false),
                    InternalPickupPositionY = table.Column<float>(nullable: false),
                    InternalPickupPositionZ = table.Column<float>(nullable: false),
                    MaxObjectCount = table.Column<short>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SpawnPossible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buildings_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buildings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrimeBots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AmphetamineCost = table.Column<decimal>(nullable: true),
                    AmphetamineCount = table.Column<int>(nullable: true),
                    AmphetamineDefaultCount = table.Column<int>(nullable: true),
                    AssaultRifleCost = table.Column<decimal>(nullable: true),
                    AssaultRifleCount = table.Column<int>(nullable: true),
                    AssaultRifleDefaultCount = table.Column<int>(nullable: true),
                    AssaultRifleMagazineCost = table.Column<decimal>(nullable: true),
                    AssaultRifleMagazineCount = table.Column<int>(nullable: true),
                    AssaultRifleMagazineDefaultCount = table.Column<int>(nullable: true),
                    AssaultRifleMk2Cost = table.Column<decimal>(nullable: true),
                    AssaultRifleMk2Count = table.Column<int>(nullable: true),
                    AssaultRifleMk2DefaultCount = table.Column<int>(nullable: true),
                    AssaultRifleMk2MagazineCost = table.Column<decimal>(nullable: true),
                    AssaultRifleMk2MagazineCount = table.Column<int>(nullable: true),
                    AssaultRifleMk2MagazineDefaultCount = table.Column<int>(nullable: true),
                    CocaineCost = table.Column<decimal>(nullable: true),
                    CocaineCount = table.Column<int>(nullable: true),
                    CocaineDefaultCount = table.Column<int>(nullable: true),
                    CombatPistolCost = table.Column<decimal>(nullable: true),
                    CombatPistolCount = table.Column<int>(nullable: true),
                    CombatPistolDefaultCount = table.Column<int>(nullable: true),
                    CombatPistolMagazineCost = table.Column<decimal>(nullable: true),
                    CombatPistolMagazineCount = table.Column<int>(nullable: true),
                    CombatPistolMagazineDefaultCount = table.Column<int>(nullable: true),
                    CrackCost = table.Column<decimal>(nullable: true),
                    CrackCount = table.Column<int>(nullable: true),
                    CrackDefaultCount = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    DoubleBarrelShotgunCost = table.Column<decimal>(nullable: true),
                    DoubleBarrelShotgunCount = table.Column<int>(nullable: true),
                    DoubleBarrelShotgunDefaultCount = table.Column<int>(nullable: true),
                    DoubleBarrelShotgunMagazineCost = table.Column<decimal>(nullable: true),
                    DoubleBarrelShotgunMagazineCount = table.Column<int>(nullable: true),
                    DoubleBarrelShotgunMagazineDefaultCount = table.Column<int>(nullable: true),
                    ExcstasyCost = table.Column<decimal>(nullable: true),
                    ExcstasyCount = table.Column<int>(nullable: true),
                    ExcstasyDefaultCount = table.Column<int>(nullable: true),
                    GroupModelId = table.Column<int>(nullable: true),
                    HasishCost = table.Column<decimal>(nullable: true),
                    HasishCount = table.Column<int>(nullable: true),
                    HasishDefaultCount = table.Column<int>(nullable: true),
                    HeavyPistolCost = table.Column<decimal>(nullable: true),
                    HeavyPistolCount = table.Column<int>(nullable: true),
                    HeavyPistolDefaultCount = table.Column<int>(nullable: true),
                    HeavyPistolMagazineCost = table.Column<decimal>(nullable: true),
                    HeavyPistolMagazineCount = table.Column<int>(nullable: true),
                    HeavyPistolMagazineDefaultCount = table.Column<int>(nullable: true),
                    HeroinCost = table.Column<decimal>(nullable: true),
                    HeroinCount = table.Column<int>(nullable: true),
                    HeroinDefaultCount = table.Column<int>(nullable: true),
                    LsdCost = table.Column<decimal>(nullable: true),
                    LsdCount = table.Column<int>(nullable: true),
                    LsdDefaultCount = table.Column<int>(nullable: true),
                    MarijuanaCost = table.Column<decimal>(nullable: true),
                    MarijuanaCount = table.Column<int>(nullable: true),
                    MarijuanaDefaultCount = table.Column<int>(nullable: true),
                    MetaamphetamineCost = table.Column<decimal>(nullable: true),
                    MetaamphetamineCount = table.Column<int>(nullable: true),
                    MetaamphetamineDefaultCount = table.Column<int>(nullable: true),
                    MicroSmgCost = table.Column<decimal>(nullable: true),
                    MicroSmgCount = table.Column<int>(nullable: true),
                    MicroSmgDefaultCount = table.Column<int>(nullable: true),
                    MicroSmgMagazineCost = table.Column<decimal>(nullable: true),
                    MicroSmgMagazineCount = table.Column<int>(nullable: true),
                    MicroSmgMagazineDefaultCount = table.Column<int>(nullable: true),
                    MiniSmgCost = table.Column<decimal>(nullable: true),
                    MiniSmgCount = table.Column<int>(nullable: true),
                    MiniSmgDefaultCount = table.Column<int>(nullable: true),
                    MiniSmgMagazineCost = table.Column<decimal>(nullable: true),
                    MiniSmgMagazineCount = table.Column<int>(nullable: true),
                    MiniSmgMagazineDefaultCount = table.Column<int>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Pistol50Cost = table.Column<decimal>(nullable: true),
                    Pistol50Count = table.Column<int>(nullable: true),
                    Pistol50DefaultCount = table.Column<int>(nullable: true),
                    Pistol50MagazineCost = table.Column<decimal>(nullable: true),
                    Pistol50MagazineCount = table.Column<int>(nullable: true),
                    Pistol50MagazineDefaultCount = table.Column<int>(nullable: true),
                    PistolCost = table.Column<decimal>(nullable: true),
                    PistolCount = table.Column<int>(nullable: true),
                    PistolDefaultCount = table.Column<int>(nullable: true),
                    PistolMagazineCost = table.Column<decimal>(nullable: true),
                    PistolMagazineCount = table.Column<int>(nullable: true),
                    PistolMagazineDefaultCount = table.Column<int>(nullable: true),
                    PistolMk2Cost = table.Column<decimal>(nullable: true),
                    PistolMk2Count = table.Column<int>(nullable: true),
                    PistolMk2DefaultCount = table.Column<int>(nullable: true),
                    PistolMk2MagazineCost = table.Column<decimal>(nullable: true),
                    PistolMk2MagazineCount = table.Column<int>(nullable: true),
                    PistolMk2RMagazineDefaultCount = table.Column<int>(nullable: true),
                    PumpShotgunCost = table.Column<decimal>(nullable: true),
                    PumpShotgunCount = table.Column<int>(nullable: true),
                    PumpShotgunDefaultCount = table.Column<int>(nullable: true),
                    PumpShotgunMagazineCost = table.Column<decimal>(nullable: true),
                    PumpShotgunMagazineCount = table.Column<int>(nullable: true),
                    PumpShotgunMagazineDefaultCount = table.Column<int>(nullable: true),
                    RevolverCost = table.Column<decimal>(nullable: true),
                    RevolverCount = table.Column<int>(nullable: true),
                    RevolverDefaultCount = table.Column<int>(nullable: true),
                    RevolverMagazineCost = table.Column<decimal>(nullable: true),
                    RevolverMagazineCount = table.Column<int>(nullable: true),
                    RevolverMagazineDefaultCount = table.Column<int>(nullable: true),
                    SawnoffShotgunCost = table.Column<decimal>(nullable: true),
                    SawnoffShotgunCount = table.Column<int>(nullable: true),
                    SawnoffShotgunDefaultCount = table.Column<int>(nullable: true),
                    SawnoffShotgunMagazineCost = table.Column<decimal>(nullable: true),
                    SawnoffShotgunMagazineCount = table.Column<int>(nullable: true),
                    SawnoffShotgunMagazineDefaultCount = table.Column<int>(nullable: true),
                    SmgCost = table.Column<decimal>(nullable: true),
                    SmgCount = table.Column<int>(nullable: true),
                    SmgDefaultCount = table.Column<int>(nullable: true),
                    SmgMagazineCost = table.Column<decimal>(nullable: true),
                    SmgMagazineCount = table.Column<int>(nullable: true),
                    SmgMagazineDefaultCount = table.Column<int>(nullable: true),
                    SmgMk2Cost = table.Column<decimal>(nullable: true),
                    SmgMk2Count = table.Column<int>(nullable: true),
                    SmgMk2DefaultCount = table.Column<int>(nullable: true),
                    SmgMk2MagazineCost = table.Column<decimal>(nullable: true),
                    SmgMk2MagazineCount = table.Column<int>(nullable: true),
                    SmgMk2MagazineDefaultCount = table.Column<int>(nullable: true),
                    SniperRifleCost = table.Column<decimal>(nullable: true),
                    SniperRifleCount = table.Column<int>(nullable: true),
                    SniperRifleDefaultCount = table.Column<int>(nullable: true),
                    SniperRifleMagazineCost = table.Column<decimal>(nullable: true),
                    SniperRifleMagazineCount = table.Column<int>(nullable: true),
                    SniperRifleMagazineDefaultCount = table.Column<int>(nullable: true),
                    SnsPistolCost = table.Column<decimal>(nullable: true),
                    SnsPistolCount = table.Column<int>(nullable: true),
                    SnsPistolDefaultCount = table.Column<int>(nullable: true),
                    SnsPistolMagazineCost = table.Column<decimal>(nullable: true),
                    SnsPistolMagazineCount = table.Column<int>(nullable: true),
                    SnsPistolMagazineDefaultCount = table.Column<int>(nullable: true),
                    Vehicle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeBots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrimeBots_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrimeBots_Groups_GroupModelId",
                        column: x => x.GroupModelId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupWarehouseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GetterId = table.Column<int>(nullable: true),
                    OrderItemsJson = table.Column<string>(nullable: true),
                    ShipmentLog = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupWarehouseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseOrders_Groups_GetterId",
                        column: x => x.GetterId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    Door1Damage = table.Column<bool>(nullable: false),
                    Door2Damage = table.Column<bool>(nullable: false),
                    Door3Damage = table.Column<bool>(nullable: false),
                    Door4Damage = table.Column<bool>(nullable: false),
                    EnginePowerMultiplier = table.Column<float>(nullable: false),
                    EngineTorqueMultiplier = table.Column<float>(nullable: false),
                    Fuel = table.Column<float>(nullable: false),
                    FuelConsumption = table.Column<float>(nullable: false),
                    FuelTank = table.Column<float>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    Health = table.Column<float>(nullable: false),
                    IsSpawned = table.Column<bool>(nullable: false),
                    Milage = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NumberPlate = table.Column<string>(nullable: true),
                    NumberPlateStyle = table.Column<int>(nullable: false),
                    PrimaryColor = table.Column<string>(nullable: true),
                    SecondaryColor = table.Column<string>(nullable: true),
                    SpawnPositionX = table.Column<float>(nullable: false),
                    SpawnPositionY = table.Column<float>(nullable: false),
                    SpawnPositionZ = table.Column<float>(nullable: false),
                    SpawnRotationX = table.Column<float>(nullable: false),
                    SpawnRotationY = table.Column<float>(nullable: false),
                    SpawnRotationZ = table.Column<float>(nullable: false),
                    VehicleHash = table.Column<string>(nullable: true),
                    WheelColor = table.Column<int>(nullable: false),
                    WheelType = table.Column<int>(nullable: false),
                    Window1Damage = table.Column<bool>(nullable: false),
                    Window2Damage = table.Column<bool>(nullable: false),
                    Window3Damage = table.Column<bool>(nullable: false),
                    Window4Damage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: true),
                    ChatRight = table.Column<bool>(nullable: false),
                    DoorsRight = table.Column<bool>(nullable: false),
                    DutyMinutes = table.Column<int>(nullable: false),
                    EightRight = table.Column<bool>(nullable: true),
                    FifthRight = table.Column<bool>(nullable: true),
                    FirstRight = table.Column<bool>(nullable: true),
                    FourthRight = table.Column<bool>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    OfferFromWarehouseRight = table.Column<bool>(nullable: false),
                    OrderFromWarehouseRight = table.Column<bool>(nullable: false),
                    PaycheckRight = table.Column<bool>(nullable: false),
                    RecrutationRight = table.Column<bool>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    SecondRight = table.Column<bool>(nullable: true),
                    SeventhRight = table.Column<bool>(nullable: true),
                    SixthRight = table.Column<bool>(nullable: true),
                    ThirdRight = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Workers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BuildingId = table.Column<int>(nullable: true),
                    CharacterId = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    FirstParameter = table.Column<int>(nullable: true),
                    FourthParameter = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    ItemEntityType = table.Column<int>(nullable: false),
                    ItemHash = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SecondParameter = table.Column<int>(nullable: true),
                    ThirdParameter = table.Column<int>(nullable: true),
                    VehicleId = table.Column<int>(nullable: true),
                    Weight = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupWarehouseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    GroupType = table.Column<int>(nullable: false),
                    ItemModelId = table.Column<int>(nullable: true),
                    MinimalCost = table.Column<decimal>(nullable: true),
                    ResetCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupWarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseItems_Items_ItemModelId",
                        column: x => x.ItemModelId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelephoneContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CellphoneId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelephoneContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelephoneContacts_Items_CellphoneId",
                        column: x => x.CellphoneId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelephoneMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CellphoneId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(maxLength: 256, nullable: true),
                    SenderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelephoneMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelephoneMessages_Items_CellphoneId",
                        column: x => x.CellphoneId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CharacterId",
                table: "Buildings",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CreatorId",
                table: "Buildings",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_GroupId",
                table: "Buildings",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountId",
                table: "Characters",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBots_CreatorId",
                table: "CrimeBots",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBots_GroupModelId",
                table: "CrimeBots",
                column: "GroupModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_CharacterId",
                table: "Descriptions",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BossCharacterId",
                table: "Groups",
                column: "BossCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseItems_ItemModelId",
                table: "GroupWarehouseItems",
                column: "ItemModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseOrders_GetterId",
                table: "GroupWarehouseOrders",
                column: "GetterId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuildingId",
                table: "Items",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterId",
                table: "Items",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatorId",
                table: "Items",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupId",
                table: "Items",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_VehicleId",
                table: "Items",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_AccountId",
                table: "Penaltlies",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_CreatorId",
                table: "Penaltlies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TelephoneContacts_CellphoneId",
                table: "TelephoneContacts",
                column: "CellphoneId");

            migrationBuilder.CreateIndex(
                name: "IX_TelephoneMessages_CellphoneId",
                table: "TelephoneMessages",
                column: "CellphoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CharacterId",
                table: "Vehicles",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CreatorId",
                table: "Vehicles",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_GroupId",
                table: "Vehicles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CharacterId",
                table: "Workers",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GroupId",
                table: "Workers",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrimeBots");

            migrationBuilder.DropTable(
                name: "Descriptions");

            migrationBuilder.DropTable(
                name: "GroupWarehouseItems");

            migrationBuilder.DropTable(
                name: "GroupWarehouseOrders");

            migrationBuilder.DropTable(
                name: "Penaltlies");

            migrationBuilder.DropTable(
                name: "TelephoneContacts");

            migrationBuilder.DropTable(
                name: "TelephoneMessages");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
