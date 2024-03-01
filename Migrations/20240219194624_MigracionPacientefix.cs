using Microsoft.EntityFrameworkCore.Migrations;

namespace Turnos.Migrations
{
    public partial class MigracionPacientefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Paciente",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Paciente",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Paciente",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Paciente",
                newName: "email");
        }
    }
}
