using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.api.Services.Context
{
    /// <inheritdoc />
    public partial class SqlServerFastReportDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataRegistro",
                value: new DateTime(2024, 5, 23, 3, 11, 23, 829, DateTimeKind.Unspecified).AddTicks(4332));

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "DataRegistro",
                value: new DateTime(2024, 5, 23, 3, 11, 23, 829, DateTimeKind.Unspecified).AddTicks(4352));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "DataNascimento", "DataRegistro", "PasswordHash" },
                values: new object[] { "25bcdc39-c039-4dd0-be63-d57b6e18c1a5", new DateTime(2024, 5, 23, 3, 11, 23, 829, DateTimeKind.Unspecified).AddTicks(4079), new DateTime(2024, 5, 23, 3, 11, 23, 829, DateTimeKind.Unspecified).AddTicks(4112), "AQAAAAIAAYagAAAAEKoc//H14VhoaGnwnRCh03f8sDx/TjnkUAAlJRcp0RWZV+sGxOldWtxbxzZN47zVFw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataRegistro",
                value: new DateTime(2024, 5, 23, 3, 10, 38, 999, DateTimeKind.Unspecified).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "DataRegistro",
                value: new DateTime(2024, 5, 23, 3, 10, 38, 999, DateTimeKind.Unspecified).AddTicks(5819));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "DataNascimento", "DataRegistro", "PasswordHash" },
                values: new object[] { "09dadb03-9e4f-4180-8941-2e47b224e96c", new DateTime(2024, 5, 23, 3, 10, 38, 999, DateTimeKind.Unspecified).AddTicks(5601), new DateTime(2024, 5, 23, 3, 10, 38, 999, DateTimeKind.Unspecified).AddTicks(5630), "AQAAAAIAAYagAAAAEMS2WvFgM2sZhENA57jVnQgD5UxuOq4Uz2+0bWQxeIpeobz78Z2bCOp+FZht88fMcw==" });
        }
    }
}
