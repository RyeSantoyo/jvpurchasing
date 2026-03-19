using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jvPo.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PO_Users_UserId",
                table: "PO");

            migrationBuilder.DropIndex(
                name: "IX_PO_UserId",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PO");

            migrationBuilder.AddColumn<string>(
                name: "AgreedTerms",
                table: "PO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "PO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "PO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierAddress",
                table: "PO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "PO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "Company",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreedTerms",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "SupplierAddress",
                table: "PO");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "PO");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PO",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyCode",
                table: "Company",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_PO_UserId",
                table: "PO",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PO_Users_UserId",
                table: "PO",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
