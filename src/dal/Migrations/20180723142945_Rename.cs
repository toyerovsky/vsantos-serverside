using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelations_CharacterRecords_Charac~",
                table: "CriminalCaseCharacterRecordRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelations_CriminalCases_CriminalC~",
                table: "CriminalCaseCharacterRecordRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelations_CriminalCases_CriminalCas~",
                table: "CriminalCaseVehicleRecordRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelations_VehicleRecordModels_Vehic~",
                table: "CriminalCaseVehicleRecordRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Buildings_BuildingId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Vehicles_OwnerVehicleId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Vehicles_TuningInVehicleId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_OwnerVehicleId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_TuningInVehicleId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CriminalCaseVehicleRecordRelations",
                table: "CriminalCaseVehicleRecordRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CriminalCaseCharacterRecordRelations",
                table: "CriminalCaseCharacterRecordRelations");

            migrationBuilder.DropColumn(
                name: "OwnerVehicleId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TuningInVehicleId",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "CriminalCaseVehicleRecordRelations",
                newName: "CriminalCaseVehicleRecordRelation");

            migrationBuilder.RenameTable(
                name: "CriminalCaseCharacterRecordRelations",
                newName: "CriminalCaseCharacterRecordRelation");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseVehicleRecordRelations_VehicleRecordId",
                table: "CriminalCaseVehicleRecordRelation",
                newName: "IX_CriminalCaseVehicleRecordRelation_VehicleRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseVehicleRecordRelations_CriminalCaseId",
                table: "CriminalCaseVehicleRecordRelation",
                newName: "IX_CriminalCaseVehicleRecordRelation_CriminalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseCharacterRecordRelations_CriminalCaseId",
                table: "CriminalCaseCharacterRecordRelation",
                newName: "IX_CriminalCaseCharacterRecordRelation_CriminalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseCharacterRecordRelations_CharacterRecordId",
                table: "CriminalCaseCharacterRecordRelation",
                newName: "IX_CriminalCaseCharacterRecordRelation_CharacterRecordId");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TuningId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CriminalCaseVehicleRecordRelation",
                table: "CriminalCaseVehicleRecordRelation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CriminalCaseCharacterRecordRelation",
                table: "CriminalCaseCharacterRecordRelation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TuningId",
                table: "Items",
                column: "TuningId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_VehicleId",
                table: "Items",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelation_CharacterRecords_Charact~",
                table: "CriminalCaseCharacterRecordRelation",
                column: "CharacterRecordId",
                principalTable: "CharacterRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelation_CriminalCases_CriminalCa~",
                table: "CriminalCaseCharacterRecordRelation",
                column: "CriminalCaseId",
                principalTable: "CriminalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelation_CriminalCases_CriminalCase~",
                table: "CriminalCaseVehicleRecordRelation",
                column: "CriminalCaseId",
                principalTable: "CriminalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelation_VehicleRecordModels_Vehicl~",
                table: "CriminalCaseVehicleRecordRelation",
                column: "VehicleRecordId",
                principalTable: "VehicleRecordModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Buildings_BuildingId",
                table: "Items",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Vehicles_TuningId",
                table: "Items",
                column: "TuningId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Vehicles_VehicleId",
                table: "Items",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelation_CharacterRecords_Charact~",
                table: "CriminalCaseCharacterRecordRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelation_CriminalCases_CriminalCa~",
                table: "CriminalCaseCharacterRecordRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelation_CriminalCases_CriminalCase~",
                table: "CriminalCaseVehicleRecordRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelation_VehicleRecordModels_Vehicl~",
                table: "CriminalCaseVehicleRecordRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Buildings_BuildingId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Vehicles_TuningId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Vehicles_VehicleId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_TuningId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_VehicleId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CriminalCaseVehicleRecordRelation",
                table: "CriminalCaseVehicleRecordRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CriminalCaseCharacterRecordRelation",
                table: "CriminalCaseCharacterRecordRelation");

            migrationBuilder.DropColumn(
                name: "TuningId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "CriminalCaseVehicleRecordRelation",
                newName: "CriminalCaseVehicleRecordRelations");

            migrationBuilder.RenameTable(
                name: "CriminalCaseCharacterRecordRelation",
                newName: "CriminalCaseCharacterRecordRelations");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseVehicleRecordRelation_VehicleRecordId",
                table: "CriminalCaseVehicleRecordRelations",
                newName: "IX_CriminalCaseVehicleRecordRelations_VehicleRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseVehicleRecordRelation_CriminalCaseId",
                table: "CriminalCaseVehicleRecordRelations",
                newName: "IX_CriminalCaseVehicleRecordRelations_CriminalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseCharacterRecordRelation_CriminalCaseId",
                table: "CriminalCaseCharacterRecordRelations",
                newName: "IX_CriminalCaseCharacterRecordRelations_CriminalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalCaseCharacterRecordRelation_CharacterRecordId",
                table: "CriminalCaseCharacterRecordRelations",
                newName: "IX_CriminalCaseCharacterRecordRelations_CharacterRecordId");

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "OwnerVehicleId",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TuningInVehicleId",
                table: "Items",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CriminalCaseVehicleRecordRelations",
                table: "CriminalCaseVehicleRecordRelations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CriminalCaseCharacterRecordRelations",
                table: "CriminalCaseCharacterRecordRelations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerVehicleId",
                table: "Items",
                column: "OwnerVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TuningInVehicleId",
                table: "Items",
                column: "TuningInVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelations_CharacterRecords_Charac~",
                table: "CriminalCaseCharacterRecordRelations",
                column: "CharacterRecordId",
                principalTable: "CharacterRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseCharacterRecordRelations_CriminalCases_CriminalC~",
                table: "CriminalCaseCharacterRecordRelations",
                column: "CriminalCaseId",
                principalTable: "CriminalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelations_CriminalCases_CriminalCas~",
                table: "CriminalCaseVehicleRecordRelations",
                column: "CriminalCaseId",
                principalTable: "CriminalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalCaseVehicleRecordRelations_VehicleRecordModels_Vehic~",
                table: "CriminalCaseVehicleRecordRelations",
                column: "VehicleRecordId",
                principalTable: "VehicleRecordModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Buildings_BuildingId",
                table: "Items",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Vehicles_OwnerVehicleId",
                table: "Items",
                column: "OwnerVehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Vehicles_TuningInVehicleId",
                table: "Items",
                column: "TuningInVehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
