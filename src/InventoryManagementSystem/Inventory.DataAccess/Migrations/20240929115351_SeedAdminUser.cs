using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("af9e5035-b4e6-46cf-aaf4-d7c55cc6d424"), 0, "1eca4518-536a-4ecb-9c2a-925facffa974", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEJIN4Srqxv5m0Qg9tPXm61T3wzRxoaxjxTsIv/jKoUGdk1NVjkyZguDWHMjW9TKKeA==", null, false, "c2faed60-736c-4156-a5c4-716bf6f154da", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { new Guid("af9e5035-b4e6-46cf-aaf4-d7c55cc6d424"), "admin@admin.com", "Admin User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("af9e5035-b4e6-46cf-aaf4-d7c55cc6d424"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("af9e5035-b4e6-46cf-aaf4-d7c55cc6d424"));
        }
    }
}
