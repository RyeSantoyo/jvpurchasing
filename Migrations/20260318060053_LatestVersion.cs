using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jvPo.Migrations
{
    /// <inheritdoc />
    public partial class LatestVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "PODetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PONumber",
                table: "PODetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "PODetails");

            migrationBuilder.DropColumn(
                name: "PONumber",
                table: "PODetails");
        }
    }
}
