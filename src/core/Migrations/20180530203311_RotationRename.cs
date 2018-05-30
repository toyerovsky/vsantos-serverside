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
