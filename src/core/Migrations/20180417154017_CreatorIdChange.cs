using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.Core.Migrations
{
    public partial class CreatorIdChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Accounts_CreatorId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_CrimeBots_Accounts_CreatorId",
                table: "CrimeBots");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Accounts_CreatorId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Accounts_CreatorId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CreatorId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Items_CreatorId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_CrimeBots_CreatorId",
                table: "CrimeBots");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_CreatorId",
                table: "Buildings");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Buildings",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Buildings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CreatorId",
                table: "Vehicles",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatorId",
                table: "Items",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeBots_CreatorId",
                table: "CrimeBots",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CreatorId",
                table: "Buildings",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Accounts_CreatorId",
                table: "Buildings",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CrimeBots_Accounts_CreatorId",
                table: "CrimeBots",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Accounts_CreatorId",
                table: "Items",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Accounts_CreatorId",
                table: "Vehicles",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
