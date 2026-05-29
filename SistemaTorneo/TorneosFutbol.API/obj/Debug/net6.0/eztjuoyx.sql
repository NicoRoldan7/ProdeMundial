CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Partidos" (
    "Id" uuid NOT NULL,
    "LocalId" uuid NOT NULL,
    "VisitanteId" uuid NOT NULL,
    "GolesLocalReal" integer NULL,
    "GolesVisitanteReal" integer NULL,
    "FechaPartido" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Partidos" PRIMARY KEY ("Id")
);

CREATE TABLE "Predicciones" (
    "Id" uuid NOT NULL,
    "UsuarioId" uuid NOT NULL,
    "PartidoId" uuid NOT NULL,
    "GolesLocalVoto" integer NOT NULL,
    "GolesVisitanteVoto" integer NOT NULL,
    "PuntosGanados" integer NOT NULL,
    CONSTRAINT "PK_Predicciones" PRIMARY KEY ("Id")
);

CREATE TABLE "Usuarios" (
    "Id" uuid NOT NULL,
    "Nombre" text NOT NULL,
    "Username" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "FechaRegistro" timestamp with time zone NOT NULL,
    "PuntajeTotal" integer NOT NULL,
    CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Usuarios_Nombre" ON "Usuarios" ("Nombre");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260529185035_AgregarCampoGrupo', '6.0.29');

COMMIT;

