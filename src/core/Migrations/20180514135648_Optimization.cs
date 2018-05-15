/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace VRP.Core.Migrations
{
    public partial class Optimization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penaltlies_Accounts_CreatorId",
                table: "Penaltlies");

            migrationBuilder.DropIndex(
                name: "IX_Penaltlies_CreatorId",
                table: "Penaltlies");

            migrationBuilder.DropColumn(
                name: "DivingEfficiency",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Force",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "RunningEfficiency",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Online",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Serial",
                table: "Accounts",
                newName: "SerialsJson");

            migrationBuilder.RenameColumn(
                name: "OtherForumGroups",
                table: "Accounts",
                newName: "SecondaryForumGroups");

            migrationBuilder.RenameColumn(
                name: "ForumGroup",
                table: "Accounts",
                newName: "PrimaryForumGroup");

            migrationBuilder.AlterColumn<short>(
                name: "Weight",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "Weight",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<DateTime>(
                name: "JobReleaseDate",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<byte>(
                name: "Height",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<byte>(
                name: "Health",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ZonePropertiesJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.RenameColumn(
                name: "SerialsJson",
                table: "Accounts",
                newName: "Serial");

            migrationBuilder.RenameColumn(
                name: "SecondaryForumGroups",
                table: "Accounts",
                newName: "OtherForumGroups");

            migrationBuilder.RenameColumn(
                name: "PrimaryForumGroup",
                table: "Accounts",
                newName: "ForumGroup");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Items",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "Weight",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<DateTime>(
                name: "JobReleaseDate",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Height",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "Health",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AddColumn<short>(
                name: "DivingEfficiency",
                table: "Characters",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Force",
                table: "Characters",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "RunningEfficiency",
                table: "Characters",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Accounts",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Penaltlies_CreatorId",
                table: "Penaltlies",
                column: "CreatorId");

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
