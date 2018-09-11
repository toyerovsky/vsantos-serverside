using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class RenameToGrant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Characters_CharacterId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Groups_GroupId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_GroupRanks_GroupRankId",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "Dotation",
                table: "Groups",
                newName: "Grant");

            migrationBuilder.AlterColumn<int>(
                name: "GroupRankId",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Workers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ItemTemplates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "ItemTemplates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BornDate",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_CreatorId",
                table: "ItemTemplates",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTemplates_Accounts_CreatorId",
                table: "ItemTemplates",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Characters_CharacterId",
                table: "Workers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Groups_GroupId",
                table: "Workers",
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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTemplates_Accounts_CreatorId",
                table: "ItemTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Characters_CharacterId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Groups_GroupId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_GroupRanks_GroupRankId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_ItemTemplates_CreatorId",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ItemTemplates");

            migrationBuilder.RenameColumn(
                name: "Grant",
                table: "Groups",
                newName: "Dotation");

            migrationBuilder.AlterColumn<int>(
                name: "GroupRankId",
                table: "Workers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Workers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CharacterId",
                table: "Workers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BornDate",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Characters_CharacterId",
                table: "Workers",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Groups_GroupId",
                table: "Workers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_GroupRanks_GroupRankId",
                table: "Workers",
                column: "GroupRankId",
                principalTable: "GroupRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
