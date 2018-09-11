using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class Salary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultRankId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultForGroupId",
                table: "GroupRanks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "GroupRanks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRanks_DefaultForGroupId",
                table: "GroupRanks",
                column: "DefaultForGroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRanks_Groups_DefaultForGroupId",
                table: "GroupRanks",
                column: "DefaultForGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRanks_Groups_DefaultForGroupId",
                table: "GroupRanks");

            migrationBuilder.DropIndex(
                name: "IX_GroupRanks_DefaultForGroupId",
                table: "GroupRanks");

            migrationBuilder.DropColumn(
                name: "DefaultRankId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DefaultForGroupId",
                table: "GroupRanks");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "GroupRanks");
        }
    }
}
