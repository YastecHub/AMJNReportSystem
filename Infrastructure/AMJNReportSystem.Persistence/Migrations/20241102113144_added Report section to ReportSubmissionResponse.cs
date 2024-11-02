using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMJNReportSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedReportsectiontoReportSubmissionResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportSubmissions_ReportSections_ReportSubmissionSectionId",
                table: "ReportSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ReportSubmissions_ReportSubmissionSectionId",
                table: "ReportSubmissions");

            migrationBuilder.DropColumn(
                name: "ReportSubmissionSectionId",
                table: "ReportSubmissions");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportSubmissionSectionId",
                table: "ReportResponses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReportResponses_ReportSubmissionSectionId",
                table: "ReportResponses",
                column: "ReportSubmissionSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportResponses_ReportSections_ReportSubmissionSectionId",
                table: "ReportResponses",
                column: "ReportSubmissionSectionId",
                principalTable: "ReportSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportResponses_ReportSections_ReportSubmissionSectionId",
                table: "ReportResponses");

            migrationBuilder.DropIndex(
                name: "IX_ReportResponses_ReportSubmissionSectionId",
                table: "ReportResponses");

            migrationBuilder.DropColumn(
                name: "ReportSubmissionSectionId",
                table: "ReportResponses");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportSubmissionSectionId",
                table: "ReportSubmissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReportSubmissions_ReportSubmissionSectionId",
                table: "ReportSubmissions",
                column: "ReportSubmissionSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportSubmissions_ReportSections_ReportSubmissionSectionId",
                table: "ReportSubmissions",
                column: "ReportSubmissionSectionId",
                principalTable: "ReportSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
