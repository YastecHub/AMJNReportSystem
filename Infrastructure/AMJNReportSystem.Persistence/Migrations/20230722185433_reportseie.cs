using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace AMJNReportSystem.Persistence.Migrations
{
    public partial class reportseie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportSubmissions_ReportSubmissionId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionWindows_ReportSubmissions_ReportSubmissionId",
                table: "SubmissionWindows");

            migrationBuilder.DropColumn(
                name: "ReporterIds",
                table: "SubmissionWindows");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "SubmissionWindows",
                newName: "IsLocked");

            migrationBuilder.RenameColumn(
                name: "ReportSubmissionId",
                table: "Reports",
                newName: "SubmissionWindowId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportSubmissionId",
                table: "Reports",
                newName: "IX_Reports_SubmissionWindowId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportSubmissionId",
                table: "SubmissionWindows",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "SubmissionWindows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportTypeId",
                table: "SubmissionWindows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "SubmissionWindows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "ReportTypeSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "ReportTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportSubmissionStatus",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReportSubmissionTimeliness",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportDataSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportTypeSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportSectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDataSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDataSections_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionWindows_ReportTypeId",
                table: "SubmissionWindows",
                column: "ReportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDataSections_ReportId",
                table: "ReportDataSections",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_SubmissionWindows_SubmissionWindowId",
                table: "Reports",
                column: "SubmissionWindowId",
                principalTable: "SubmissionWindows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionWindows_ReportSubmissions_ReportSubmissionId",
                table: "SubmissionWindows",
                column: "ReportSubmissionId",
                principalTable: "ReportSubmissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionWindows_ReportTypes_ReportTypeId",
                table: "SubmissionWindows",
                column: "ReportTypeId",
                principalTable: "ReportTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_SubmissionWindows_SubmissionWindowId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionWindows_ReportSubmissions_ReportSubmissionId",
                table: "SubmissionWindows");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionWindows_ReportTypes_ReportTypeId",
                table: "SubmissionWindows");

            migrationBuilder.DropTable(
                name: "ReportDataSections");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionWindows_ReportTypeId",
                table: "SubmissionWindows");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "SubmissionWindows");

            migrationBuilder.DropColumn(
                name: "ReportTypeId",
                table: "SubmissionWindows");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "SubmissionWindows");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "ReportTypeSections");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "ReportTypes");

            migrationBuilder.DropColumn(
                name: "ReportSubmissionStatus",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportSubmissionTimeliness",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "IsLocked",
                table: "SubmissionWindows",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "SubmissionWindowId",
                table: "Reports",
                newName: "ReportSubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_SubmissionWindowId",
                table: "Reports",
                newName: "IX_Reports_ReportSubmissionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportSubmissionId",
                table: "SubmissionWindows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReporterIds",
                table: "SubmissionWindows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportSubmissions_ReportSubmissionId",
                table: "Reports",
                column: "ReportSubmissionId",
                principalTable: "ReportSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionWindows_ReportSubmissions_ReportSubmissionId",
                table: "SubmissionWindows",
                column: "ReportSubmissionId",
                principalTable: "ReportSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
