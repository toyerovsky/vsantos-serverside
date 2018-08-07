using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class GroupRanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Characters_CharacterId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsSpawned",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "GroupRankId",
                table: "Workers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GroupRanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Rights = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRanks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GroupRankId",
                table: "Workers",
                column: "GroupRankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Characters_CharacterId",
                table: "Vehicles",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_GroupRanks_GroupRankId",
                table: "Workers",
                column: "GroupRankId",
                principalTable: "GroupRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Characters_CharacterId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_GroupRanks_GroupRankId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "GroupRanks");

            migrationBuilder.DropIndex(
                name: "IX_Workers_GroupRankId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "GroupRankId",
                table: "Workers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsSpawned",
                table: "Vehicles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Characters_CharacterId",
                table: "Vehicles",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
