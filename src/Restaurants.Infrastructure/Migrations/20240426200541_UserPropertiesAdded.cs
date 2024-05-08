﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPropertiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "HavePassport",
                table: "AspNetUsers",
                type: "bit",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DateOfBirth", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "HavePassport", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "Nationality", table: "AspNetUsers");
        }
    }
}
