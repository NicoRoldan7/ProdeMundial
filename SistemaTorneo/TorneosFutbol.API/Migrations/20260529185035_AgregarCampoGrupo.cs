using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneosFutbol.API.Migrations
{
    public partial class AgregarCampoGrupo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalId = table.Column<Guid>(type: "uuid", nullable: false),
                    VisitanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    GolesLocalReal = table.Column<int>(type: "integer", nullable: true),
                    GolesVisitanteReal = table.Column<int>(type: "integer", nullable: true),
                    FechaPartido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predicciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    GolesLocalVoto = table.Column<int>(type: "integer", nullable: false),
                    GolesVisitanteVoto = table.Column<int>(type: "integer", nullable: false),
                    PuntosGanados = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predicciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PuntajeTotal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nombre",
                table: "Usuarios",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partidos");

            migrationBuilder.DropTable(
                name: "Predicciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
