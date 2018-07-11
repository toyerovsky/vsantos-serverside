using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class RemoveBlockage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Penaltlies_CharacterBlockageId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterBlockageId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterBlockageId",
                table: "Characters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterBlockageId",
                table: "Characters",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
