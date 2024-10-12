using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMJNReportSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatesondb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ReportTypes");

            migrationBuilder.DropColumn(
                name: "ReportTag",
                table: "ReportTypes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ReportTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ReportTypes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportTag",
                table: "ReportTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ReportTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
