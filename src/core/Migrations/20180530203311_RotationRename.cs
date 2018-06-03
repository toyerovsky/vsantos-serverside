/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VRP.Core.Migrations
{
    public partial class RotationRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastPositionRotZ",
                table: "Characters",
                newName: "LastRotationZ");

            migrationBuilder.RenameColumn(
                name: "LastPositionRotY",
                table: "Characters",
                newName: "LastRotationY");

            migrationBuilder.RenameColumn(
                name: "LastPositionRotX",
                table: "Characters",
                newName: "LastRotationX");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastRotationZ",
                table: "Characters",
                newName: "LastPositionRotZ");

            migrationBuilder.RenameColumn(
                name: "LastRotationY",
                table: "Characters",
                newName: "LastPositionRotY");

            migrationBuilder.RenameColumn(
                name: "LastRotationX",
                table: "Characters",
                newName: "LastPositionRotX");
        }
    }
}
