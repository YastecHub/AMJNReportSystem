using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMJNReportSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportresponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportResponses_ReportSubmissions_ReportSubmissionId",
                table: "ReportResponses");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportSubmissionId",
                table: "ReportResponses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportResponses_ReportSubmissions_ReportSubmissionId",
                table: "ReportResponses",
                column: "ReportSubmissionId",
                principalTable: "ReportSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportResponses_ReportSubmissions_ReportSubmissionId",
                table: "ReportResponses");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportSubmissionId",
                table: "ReportResponses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportResponses_ReportSubmissions_ReportSubmissionId",
                table: "ReportResponses",
                column: "ReportSubmissionId",
                principalTable: "ReportSubmissions",
                principalColumn: "Id");
        }
    }
}
