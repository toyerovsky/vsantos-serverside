using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VRP.DAL.Migrations
{
    public partial class Creator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Characters_BossCharacterId",
                table: "Groups");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deactivated",
                table: "Penaltlies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivationDate",
                table: "Penaltlies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "BossCharacterId",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Groups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImageUploadDate",
                table: "Groups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupRanks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImageUploadDate",
                table: "Characters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Characters",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Buildings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Buildings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CreatorId",
                table: "Vehicles",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRanks_GroupId",
                table: "GroupRanks",
                column: "GroupId");

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
                name: "FK_GroupRanks_Groups_GroupId",
                table: "GroupRanks",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Characters_BossCharacterId",
                table: "Groups",
                column: "BossCharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Accounts_CreatorId",
                table: "Vehicles",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Accounts_CreatorId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRanks_Groups_GroupId",
                table: "GroupRanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Characters_BossCharacterId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Accounts_CreatorId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CreatorId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_GroupRanks_GroupId",
                table: "GroupRanks");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_CreatorId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Deactivated",
                table: "Penaltlies");

            migrationBuilder.DropColumn(
                name: "DeactivationDate",
                table: "Penaltlies");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ImageUploadDate",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupRanks");

            migrationBuilder.DropColumn(
                name: "ImageUploadDate",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Buildings");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BossCharacterId",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Buildings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Characters_BossCharacterId",
                table: "Groups",
                column: "BossCharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
