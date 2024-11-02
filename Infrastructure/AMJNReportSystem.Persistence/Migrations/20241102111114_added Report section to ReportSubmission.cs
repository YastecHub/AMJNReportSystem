using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMJNReportSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedReportsectiontoReportSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
