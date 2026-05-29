using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneosFutbol.Infrastructure.Migrations
{
    public partial class AgregarGrupoAEquipos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Partidos",
                table: "Partidos");

            migrationBuilder.RenameTable(
                name: "Partidos",
                newName: "partidos");

            migrationBuilder.RenameColumn(
                name: "Finalizado",
                table: "partidos",
                newName: "finalizado");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "partidos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VisitanteId",
                table: "partidos",
                newName: "visitante_id");

            migrationBuilder.RenameColumn(
                name: "LocalId",
                table: "partidos",
                newName: "local_id");

            migrationBuilder.RenameColumn(
                name: "GolesVisitanteReal",
                table: "partidos",
                newName: "goles_visitante_real");

            migrationBuilder.RenameColumn(
                name: "GolesLocalReal",
                table: "partidos",
                newName: "goles_local_real");

            migrationBuilder.RenameColumn(
                name: "FechaId",
                table: "partidos",
                newName: "fecha_id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Fechas",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Fechas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Equipos",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Equipos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Equipos",
                newName: "logo_url");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grupo",
                table: "Equipos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_partidos",
                table: "partidos",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_partidos",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Grupo",
                table: "Equipos");

            migrationBuilder.RenameTable(
                name: "partidos",
                newName: "Partidos");

            migrationBuilder.RenameColumn(
                name: "finalizado",
                table: "Partidos",
                newName: "Finalizado");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Partidos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "visitante_id",
                table: "Partidos",
                newName: "VisitanteId");

            migrationBuilder.RenameColumn(
                name: "local_id",
                table: "Partidos",
                newName: "LocalId");

            migrationBuilder.RenameColumn(
                name: "goles_visitante_real",
                table: "Partidos",
                newName: "GolesVisitanteReal");

            migrationBuilder.RenameColumn(
                name: "goles_local_real",
                table: "Partidos",
                newName: "GolesLocalReal");

            migrationBuilder.RenameColumn(
                name: "fecha_id",
                table: "Partidos",
                newName: "FechaId");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Fechas",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Fechas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Equipos",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Equipos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "logo_url",
                table: "Equipos",
                newName: "LogoUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partidos",
                table: "Partidos",
                column: "Id");
        }
    }
}
