using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace TourGuide.Migrations
{
    public partial class AddMtM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Point_Point_PointId", table: "Point");
            migrationBuilder.DropColumn(name: "PointId", table: "Point");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointId",
                table: "Point",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Point_Point_PointId",
                table: "Point",
                column: "PointId",
                principalTable: "Point",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
