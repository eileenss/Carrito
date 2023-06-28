using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carrito_D.Data.Migrations
{
    public partial class Secuencia_legajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "Legajo",
                startValue: 1000L);

            migrationBuilder.AlterColumn<int>(
                name: "Legajo",
                table: "Personas",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR Legajo",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "Legajo");

            migrationBuilder.AlterColumn<int>(
                name: "Legajo",
                table: "Personas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR Legajo");
        }
    }
}
