using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EczaneV3.Data.Migrations
{
    public partial class mig_22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Brand_BrandId",
                table: "Medicaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Category_CategoryId",
                table: "Medicaments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Medicaments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Medicaments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Medicaments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Medicaments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Brand_BrandId",
                table: "Medicaments",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Category_CategoryId",
                table: "Medicaments",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
