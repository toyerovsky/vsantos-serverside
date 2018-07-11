using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class CharacterBlockage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Penaltlies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CharacterBlockageId",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_CharacterId",
                table: "Penaltlies",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterBlockageId",
                table: "Characters",
                column: "CharacterBlockageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Penaltlies_CharacterBlockageId",
                table: "Characters",
                column: "CharacterBlockageId",
                principalTable: "Penaltlies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Characters_CharacterId",
                table: "Penaltlies",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Penaltlies_CharacterBlockageId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Characters_CharacterId",
                table: "Penaltlies");

            migrationBuilder.DropIndex(
                name: "IX_Penaltlies_CharacterId",
                table: "Penaltlies");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterBlockageId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Penaltlies");

            migrationBuilder.DropColumn(
                name: "CharacterBlockageId",
                table: "Characters");
        }
    }
}
