using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreCrewApp.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditLogID",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Local).AddTicks(2989));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditLogID",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Local).AddTicks(2992));

            migrationBuilder.UpdateData(
                table: "EmployeeBenefits",
                keyColumns: new[] { "BenefitID", "EmployeeID" },
                keyValues: new object[] { 1, 1 },
                column: "EnrollmentDate",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "EmployeeBenefits",
                keyColumns: new[] { "BenefitID", "EmployeeID" },
                keyValues: new object[] { 2, 2 },
                column: "EnrollmentDate",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Local).AddTicks(2839));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationID",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Utc).AddTicks(2961));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationID",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 11, 12, 16, 11, 10, 350, DateTimeKind.Utc).AddTicks(2965));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditLogID",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Local).AddTicks(7802));

            migrationBuilder.UpdateData(
                table: "AuditLogs",
                keyColumn: "AuditLogID",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Local).AddTicks(7808));

            migrationBuilder.UpdateData(
                table: "EmployeeBenefits",
                keyColumns: new[] { "BenefitID", "EmployeeID" },
                keyValues: new object[] { 1, 1 },
                column: "EnrollmentDate",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Local).AddTicks(7568));

            migrationBuilder.UpdateData(
                table: "EmployeeBenefits",
                keyColumns: new[] { "BenefitID", "EmployeeID" },
                keyValues: new object[] { 2, 2 },
                column: "EnrollmentDate",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Local).AddTicks(7626));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationID",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Utc).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationID",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2024, 11, 5, 11, 20, 47, 931, DateTimeKind.Utc).AddTicks(7767));
        }
    }
}
