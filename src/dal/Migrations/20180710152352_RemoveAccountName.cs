using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class RemoveAccountName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                maxLength: 50,
                nullable: true);
        }
    }
}
