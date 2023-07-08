using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carrito_D.Data.Migrations
{
    public partial class camposUnicosaddmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Personas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sucursales_Direccion",
                table: "Sucursales",
                column: "Direccion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Nombre",
                table: "Productos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_DNI",
                table: "Personas",
                column: "DNI",
                unique: true,
                filter: "[DNI] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sucursales_Direccion",
                table: "Sucursales");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Nombre",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Personas_DNI",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
