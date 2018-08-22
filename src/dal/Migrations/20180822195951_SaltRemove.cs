using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class SaltRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Accounts",
                nullable: true);
        }
    }
}
