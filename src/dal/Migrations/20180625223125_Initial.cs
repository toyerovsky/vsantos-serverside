using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
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
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ForumUserId = table.Column<long>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    PrimaryForumGroup = table.Column<long>(nullable: false),
                    SecondaryForumGroups = table.Column<string>(nullable: true),
                    SocialClub = table.Column<string>(maxLength: 50, nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: false),
                    ServerRank = table.Column<int>(nullable: false),
                    SerialsJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.UniqueConstraint("AK_Accounts_ForumUserId_Id", x => new { x.ForumUserId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "AutoSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoSales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    BornDate = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Race = table.Column<int>(nullable: false),
                    EyeColor = table.Column<string>(nullable: false),
                    FingerPrints = table.Column<string>(nullable: true),
                    DNACode = table.Column<Guid>(nullable: false),
                    Wanted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CriminalCases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriminalCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Weight = table.Column<short>(nullable: false),
                    ItemHash = table.Column<string>(nullable: true),
                    FirstParameter = table.Column<int>(nullable: true),
                    SecondParameter = table.Column<int>(nullable: true),
                    ThirdParameter = table.Column<int>(nullable: true),
                    FourthParameter = table.Column<int>(nullable: true),
                    ItemEntityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartTimeJobModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobType = table.Column<int>(nullable: false),
                    DailyMoneyLimit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartTimeJobModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    ZoneType = table.Column<int>(nullable: false),
                    ZonePropertiesJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Online = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    TodayPlayedTime = table.Column<TimeSpan>(nullable: false),
                    PlayedTime = table.Column<TimeSpan>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Money = table.Column<decimal>(nullable: false),
                    BankAccountNumber = table.Column<int>(nullable: true),
                    BankMoney = table.Column<decimal>(nullable: false),
                    Gender = table.Column<bool>(nullable: false),
                    BornDate = table.Column<DateTime>(nullable: true),
                    HasIdCard = table.Column<bool>(nullable: false),
                    HasDrivingLicense = table.Column<bool>(nullable: false),
                    IsAlive = table.Column<bool>(nullable: false),
                    Health = table.Column<byte>(nullable: false),
                    LastPositionX = table.Column<float>(nullable: false),
                    LastPositionY = table.Column<float>(nullable: false),
                    LastPositionZ = table.Column<float>(nullable: false),
                    LastRotationX = table.Column<float>(nullable: false),
                    LastRotationY = table.Column<float>(nullable: false),
                    LastRotationZ = table.Column<float>(nullable: false),
                    CurrentDimension = table.Column<uint>(nullable: false),
                    MinutesToRespawn = table.Column<int>(nullable: false),
                    CharacterLookId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    PartTimeJobWorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penaltlies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(maxLength: 256, nullable: true),
                    PenaltyType = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "VehicleRecordModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumberPlate = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    Towed = table.Column<bool>(nullable: false),
                    Wanted = table.Column<bool>(nullable: false),
                    SpecialFeatures = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRecordModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRecordModels_CharacterRecords_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "CharacterRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CriminalCaseCharacterRecordRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CriminalCaseId = table.Column<int>(nullable: true),
                    CharacterRecordId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriminalCaseCharacterRecordRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriminalCaseCharacterRecordRelations_CharacterRecords_Charac~",
                        column: x => x.CharacterRecordId,
                        principalTable: "CharacterRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriminalCaseCharacterRecordRelations_CriminalCases_CriminalC~",
                        column: x => x.CriminalCaseId,
                        principalTable: "CriminalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartTimeJobEmployerModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PositionX = table.Column<float>(nullable: false),
                    PositionY = table.Column<float>(nullable: false),
                    PositionZ = table.Column<float>(nullable: false),
                    RotationX = table.Column<float>(nullable: false),
                    RotationY = table.Column<float>(nullable: false),
                    RotationZ = table.Column<float>(nullable: false),
                    PartTimeJobId = table.Column<int>(nullable: false),
                    PartTimeJobModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartTimeJobEmployerModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartTimeJobEmployerModel_PartTimeJobModel_PartTimeJobModelId",
                        column: x => x.PartTimeJobModelId,
                        principalTable: "PartTimeJobModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterLooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessoryId = table.Column<byte>(nullable: true),
                    AccessoryTexture = table.Column<byte>(nullable: true),
                    EarsId = table.Column<byte>(nullable: true),
                    EarsTexture = table.Column<byte>(nullable: true),
                    EyebrowsId = table.Column<byte>(nullable: true),
                    EyeBrowsOpacity = table.Column<float>(nullable: true),
                    FatherId = table.Column<byte>(nullable: true),
                    ShoesId = table.Column<byte>(nullable: true),
                    ShoesTexture = table.Column<byte>(nullable: true),
                    FirstEyebrowsColor = table.Column<byte>(nullable: true),
                    FirstLipstickColor = table.Column<byte>(nullable: true),
                    FirstMakeupColor = table.Column<byte>(nullable: true),
                    GlassesId = table.Column<byte>(nullable: true),
                    GlassesTexture = table.Column<byte>(nullable: true),
                    HairId = table.Column<byte>(nullable: true),
                    HairTexture = table.Column<byte>(nullable: true),
                    HairColor = table.Column<byte>(nullable: true),
                    HatId = table.Column<byte>(nullable: true),
                    HatTexture = table.Column<byte>(nullable: true),
                    LegsId = table.Column<byte>(nullable: true),
                    LegsTexture = table.Column<byte>(nullable: true),
                    LipstickOpacity = table.Column<float>(nullable: true),
                    MakeupId = table.Column<byte>(nullable: true),
                    MakeupOpacity = table.Column<float>(nullable: true),
                    MotherId = table.Column<byte>(nullable: true),
                    SecondEyebrowsColor = table.Column<byte>(nullable: true),
                    SecondLipstickColor = table.Column<byte>(nullable: true),
                    SecondMakeupColor = table.Column<byte>(nullable: true),
                    ShapeMix = table.Column<float>(nullable: true),
                    TopId = table.Column<byte>(nullable: true),
                    TopTexture = table.Column<byte>(nullable: true),
                    TorsoId = table.Column<byte>(nullable: true),
                    UndershirtId = table.Column<byte>(nullable: true),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterLooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterLooks_Characters_CharacterId",
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
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Dotation = table.Column<int>(nullable: false),
                    MaxPayday = table.Column<int>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    GroupType = table.Column<int>(nullable: false),
                    BossCharacterId = table.Column<int>(nullable: true)
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
                name: "CriminalCaseVehicleRecordRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CriminalCaseId = table.Column<int>(nullable: true),
                    VehicleRecordId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriminalCaseVehicleRecordRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriminalCaseVehicleRecordRelations_CriminalCases_CriminalCas~",
                        column: x => x.CriminalCaseId,
                        principalTable: "CriminalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CriminalCaseVehicleRecordRelations_VehicleRecordModels_Vehic~",
                        column: x => x.VehicleRecordId,
                        principalTable: "VehicleRecordModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartTimeJobWorkerModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Salary = table.Column<decimal>(nullable: false),
                    PartTimeJobEmployerId = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false),
                    PartTimeJobEmployerModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartTimeJobWorkerModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartTimeJobWorkerModel_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartTimeJobWorkerModel_PartTimeJobEmployerModel_PartTimeJobE~",
                        column: x => x.PartTimeJobEmployerModelId,
                        principalTable: "PartTimeJobEmployerModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    AutoRenewal = table.Column<bool>(nullable: false),
                    LeaserGroupId = table.Column<int>(nullable: true),
                    LeaserCharacterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreements_Characters_LeaserCharacterId",
                        column: x => x.LeaserCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agreements_Groups_LeaserGroupId",
                        column: x => x.LeaserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    EnterCharge = table.Column<decimal>(nullable: true),
                    ExternalPickupPositionX = table.Column<float>(nullable: false),
                    ExternalPickupPositionY = table.Column<float>(nullable: false),
                    ExternalPickupPositionZ = table.Column<float>(nullable: false),
                    InternalPickupPositionX = table.Column<float>(nullable: false),
                    InternalPickupPositionY = table.Column<float>(nullable: false),
                    InternalPickupPositionZ = table.Column<float>(nullable: false),
                    MaxObjectCount = table.Column<short>(nullable: false),
                    CurrentObjectCount = table.Column<short>(nullable: false),
                    SpawnPossible = table.Column<bool>(nullable: false),
                    HasCctv = table.Column<bool>(nullable: false),
                    HasSafe = table.Column<bool>(nullable: false),
                    InternalDimension = table.Column<uint>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AutoSaleId = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_AutoSales_AutoSaleId",
                        column: x => x.AutoSaleId,
                        principalTable: "AutoSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buildings_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    GroupModelId = table.Column<int>(nullable: true),
                    VehicleModel = table.Column<string>(nullable: true),
                    PedSkin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeBots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrimeBots_Groups_GroupModelId",
                        column: x => x.GroupModelId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupWarehouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupWarehouses_Groups_GroupId",
                        column: x => x.GroupId,
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
                    GroupId = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    NumberPlate = table.Column<string>(nullable: true),
                    NumberPlateStyle = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VehicleHash = table.Column<string>(nullable: true),
                    SpawnPositionX = table.Column<float>(nullable: false),
                    SpawnPositionY = table.Column<float>(nullable: false),
                    SpawnPositionZ = table.Column<float>(nullable: false),
                    SpawnRotationX = table.Column<float>(nullable: false),
                    SpawnRotationY = table.Column<float>(nullable: false),
                    SpawnRotationZ = table.Column<float>(nullable: false),
                    IsSpawned = table.Column<bool>(nullable: false),
                    EnginePowerMultiplier = table.Column<float>(nullable: false),
                    EngineTorqueMultiplier = table.Column<float>(nullable: false),
                    Health = table.Column<float>(nullable: false),
                    Milage = table.Column<float>(nullable: false),
                    Fuel = table.Column<float>(nullable: false),
                    FuelTank = table.Column<float>(nullable: false),
                    FuelConsumption = table.Column<float>(nullable: false),
                    Door1Damage = table.Column<bool>(nullable: false),
                    Door2Damage = table.Column<bool>(nullable: false),
                    Door3Damage = table.Column<bool>(nullable: false),
                    Door4Damage = table.Column<bool>(nullable: false),
                    Window1Damage = table.Column<bool>(nullable: false),
                    Window2Damage = table.Column<bool>(nullable: false),
                    Window3Damage = table.Column<bool>(nullable: false),
                    Window4Damage = table.Column<bool>(nullable: false),
                    PrimaryColor = table.Column<string>(nullable: true),
                    SecondaryColor = table.Column<string>(nullable: true),
                    WheelType = table.Column<int>(nullable: false),
                    WheelColor = table.Column<int>(nullable: false),
                    AutoSaleId = table.Column<int>(nullable: false),
                    PartTimeJobModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_AutoSales_AutoSaleId",
                        column: x => x.AutoSaleId,
                        principalTable: "AutoSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_PartTimeJobModel_PartTimeJobModelId",
                        column: x => x.PartTimeJobModelId,
                        principalTable: "PartTimeJobModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Salary = table.Column<decimal>(nullable: false),
                    DutyMinutes = table.Column<int>(nullable: false),
                    Rights = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    CharacterId = table.Column<int>(nullable: true)
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
                name: "CrimeBotItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ResetCount = table.Column<int>(nullable: false),
                    ItemTemplateModelId = table.Column<int>(nullable: true),
                    CrimeBotModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeBotItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrimeBotItems_CrimeBots_CrimeBotModelId",
                        column: x => x.CrimeBotModelId,
                        principalTable: "CrimeBots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrimeBotItems_ItemTemplates_ItemTemplateModelId",
                        column: x => x.ItemTemplateModelId,
                        principalTable: "ItemTemplates",
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
                    ResetCount = table.Column<int>(nullable: false),
                    ItemTemplateModelId = table.Column<int>(nullable: true),
                    GroupType = table.Column<int>(nullable: false),
                    GroupWarehouseModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupWarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseItems_GroupWarehouses_GroupWarehouseModelId",
                        column: x => x.GroupWarehouseModelId,
                        principalTable: "GroupWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseItems_ItemTemplates_ItemTemplateModelId",
                        column: x => x.ItemTemplateModelId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupWarehouseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShipmentLog = table.Column<string>(nullable: true),
                    OrderedItemCount = table.Column<int>(nullable: false),
                    OrderedItemTemplateId = table.Column<int>(nullable: true),
                    GetterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupWarehouseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseOrders_GroupWarehouses_GetterId",
                        column: x => x.GetterId,
                        principalTable: "GroupWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupWarehouseOrders_ItemTemplates_OrderedItemTemplateId",
                        column: x => x.OrderedItemTemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Descriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CharacterId = table.Column<int>(nullable: true),
                    VehicleId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Descriptions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Weight = table.Column<short>(nullable: false),
                    ItemHash = table.Column<string>(nullable: true),
                    FirstParameter = table.Column<int>(nullable: true),
                    SecondParameter = table.Column<int>(nullable: true),
                    ThirdParameter = table.Column<int>(nullable: true),
                    FourthParameter = table.Column<int>(nullable: true),
                    ItemEntityType = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: true),
                    BuildingId = table.Column<int>(nullable: true),
                    OwnerVehicleId = table.Column<int>(nullable: true),
                    TuningInVehicleId = table.Column<int>(nullable: true)
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
                        name: "FK_Items_Vehicles_OwnerVehicleId",
                        column: x => x.OwnerVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Vehicles_TuningInVehicleId",
                        column: x => x.TuningInVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: false),
                    ChargeFrequency = table.Column<TimeSpan>(nullable: false),
                    AgreementId = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: true),
                    BuildingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leases_Agreements_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "Agreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leases_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TelephoneContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    CellphoneId = table.Column<int>(nullable: true)
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
                    Content = table.Column<string>(maxLength: 256, nullable: true),
                    SenderNumber = table.Column<int>(nullable: false),
                    CellphoneId = table.Column<int>(nullable: true)
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
                name: "IX_Agreements_LeaserCharacterId",
                table: "Agreements",
                column: "LeaserCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_LeaserGroupId",
                table: "Agreements",
                column: "LeaserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AutoSaleId",
                table: "Buildings",
                column: "AutoSaleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CharacterId",
                table: "Buildings",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_GroupId",
                table: "Buildings",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLooks_CharacterId",
                table: "CharacterLooks",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountId",
                table: "Characters",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBotItems_CrimeBotModelId",
                table: "CrimeBotItems",
                column: "CrimeBotModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBotItems_ItemTemplateModelId",
                table: "CrimeBotItems",
                column: "ItemTemplateModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBots_GroupModelId",
                table: "CrimeBots",
                column: "GroupModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CriminalCaseCharacterRecordRelations_CharacterRecordId",
                table: "CriminalCaseCharacterRecordRelations",
                column: "CharacterRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_CriminalCaseCharacterRecordRelations_CriminalCaseId",
                table: "CriminalCaseCharacterRecordRelations",
                column: "CriminalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CriminalCaseVehicleRecordRelations_CriminalCaseId",
                table: "CriminalCaseVehicleRecordRelations",
                column: "CriminalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CriminalCaseVehicleRecordRelations_VehicleRecordId",
                table: "CriminalCaseVehicleRecordRelations",
                column: "VehicleRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_CharacterId",
                table: "Descriptions",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_VehicleId",
                table: "Descriptions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BossCharacterId",
                table: "Groups",
                column: "BossCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseItems_GroupWarehouseModelId",
                table: "GroupWarehouseItems",
                column: "GroupWarehouseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseItems_ItemTemplateModelId",
                table: "GroupWarehouseItems",
                column: "ItemTemplateModelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseOrders_GetterId",
                table: "GroupWarehouseOrders",
                column: "GetterId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouseOrders_OrderedItemTemplateId",
                table: "GroupWarehouseOrders",
                column: "OrderedItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupWarehouses_GroupId",
                table: "GroupWarehouses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuildingId",
                table: "Items",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterId",
                table: "Items",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerVehicleId",
                table: "Items",
                column: "OwnerVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TuningInVehicleId",
                table: "Items",
                column: "TuningInVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_AgreementId",
                table: "Leases",
                column: "AgreementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leases_BuildingId",
                table: "Leases",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_VehicleId",
                table: "Leases",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PartTimeJobEmployerModel_PartTimeJobModelId",
                table: "PartTimeJobEmployerModel",
                column: "PartTimeJobModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PartTimeJobWorkerModel_CharacterId",
                table: "PartTimeJobWorkerModel",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartTimeJobWorkerModel_PartTimeJobEmployerModelId",
                table: "PartTimeJobWorkerModel",
                column: "PartTimeJobEmployerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_AccountId",
                table: "Penaltlies",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TelephoneContacts_CellphoneId",
                table: "TelephoneContacts",
                column: "CellphoneId");

            migrationBuilder.CreateIndex(
                name: "IX_TelephoneMessages_CellphoneId",
                table: "TelephoneMessages",
                column: "CellphoneId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRecordModels_OwnerId",
                table: "VehicleRecordModels",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AutoSaleId",
                table: "Vehicles",
                column: "AutoSaleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CharacterId",
                table: "Vehicles",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_GroupId",
                table: "Vehicles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PartTimeJobModelId",
                table: "Vehicles",
                column: "PartTimeJobModelId");

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
                name: "CharacterLooks");

            migrationBuilder.DropTable(
                name: "CrimeBotItems");

            migrationBuilder.DropTable(
                name: "CriminalCaseCharacterRecordRelations");

            migrationBuilder.DropTable(
                name: "CriminalCaseVehicleRecordRelations");

            migrationBuilder.DropTable(
                name: "Descriptions");

            migrationBuilder.DropTable(
                name: "GroupWarehouseItems");

            migrationBuilder.DropTable(
                name: "GroupWarehouseOrders");

            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "PartTimeJobWorkerModel");

            migrationBuilder.DropTable(
                name: "Penaltlies");

            migrationBuilder.DropTable(
                name: "TelephoneContacts");

            migrationBuilder.DropTable(
                name: "TelephoneMessages");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "CrimeBots");

            migrationBuilder.DropTable(
                name: "CriminalCases");

            migrationBuilder.DropTable(
                name: "VehicleRecordModels");

            migrationBuilder.DropTable(
                name: "GroupWarehouses");

            migrationBuilder.DropTable(
                name: "ItemTemplates");

            migrationBuilder.DropTable(
                name: "Agreements");

            migrationBuilder.DropTable(
                name: "PartTimeJobEmployerModel");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "CharacterRecords");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AutoSales");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "PartTimeJobModel");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
