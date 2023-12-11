using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EczaneV3.Data.Migrations
{
    public partial class mig_16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "images",
                table: "Medicaments",
                newName: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Medicaments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Medicaments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Medicaments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Medicaments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_BrandId",
                table: "Medicaments",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_CategoryId",
                table: "Medicaments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Brand_BrandId",
                table: "Medicaments",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Category_CategoryId",
                table: "Medicaments",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Brand_BrandId",
                table: "Medicaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Category_CategoryId",
                table: "Medicaments");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_BrandId",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_CategoryId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Medicaments");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Medicaments",
                newName: "images");
        }
    }
}
