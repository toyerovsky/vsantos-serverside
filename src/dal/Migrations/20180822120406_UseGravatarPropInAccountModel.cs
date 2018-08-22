using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class UseGravatarPropInAccountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseGravatar",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseGravatar",
                table: "Accounts");
        }
    }
}
