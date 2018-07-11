using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class PenaltyCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_AccountId",
                table: "Penaltlies");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Penaltlies",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_CreatorId",
                table: "Penaltlies",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_AccountId",
                table: "Penaltlies",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_AccountId",
                table: "Penaltlies");

            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies");

            migrationBuilder.DropIndex(
                name: "IX_Penaltlies_CreatorId",
                table: "Penaltlies");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Penaltlies",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Penaltlies_Accounts_AccountId",
                table: "Penaltlies",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
