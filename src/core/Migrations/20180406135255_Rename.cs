/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace VRP.Core.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HitPoints",
                table: "Characters",
                newName: "Health");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TodayPlayedTime",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "PlayedTime",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "CurrentDimension",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Health",
                table: "Characters",
                newName: "HitPoints");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TodayPlayedTime",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlayedTime",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AlterColumn<int>(
                name: "CurrentDimension",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(uint));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
