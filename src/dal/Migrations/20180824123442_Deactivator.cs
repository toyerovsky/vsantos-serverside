using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class Deactivator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Penaltlies",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatorId",
                table: "Penaltlies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_DeactivatorId",
                table: "Penaltlies",
                column: "DeactivatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_DeactivatorId",
                table: "Penaltlies",
                column: "DeactivatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies");

            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_DeactivatorId",
                table: "Penaltlies");

            migrationBuilder.DropIndex(
                name: "IX_Penaltlies_DeactivatorId",
                table: "Penaltlies");

            migrationBuilder.DropColumn(
                name: "DeactivatorId",
                table: "Penaltlies");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Penaltlies",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
