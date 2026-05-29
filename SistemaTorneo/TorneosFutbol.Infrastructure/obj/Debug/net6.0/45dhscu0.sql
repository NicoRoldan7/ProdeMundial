CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Equipos" (
    "Id" uuid NOT NULL,
    "Nombre" character varying(100) NOT NULL,
    "LogoUrl" character varying(500) NOT NULL,
    CONSTRAINT "PK_Equipos" PRIMARY KEY ("Id")
);

CREATE TABLE "Fechas" (
    "Id" uuid NOT NULL,
    "Nombre" character varying(100) NOT NULL,
    "Orden" integer NOT NULL,
    CONSTRAINT "PK_Fechas" PRIMARY KEY ("Id")
);

CREATE TABLE "Partidos" (
    "Id" uuid NOT NULL,
    "FechaId" uuid NOT NULL,
    "LocalId" uuid NOT NULL,
    "VisitanteId" uuid NOT NULL,
    "GolesLocalReal" integer NULL,
    "GolesVisitanteReal" integer NULL,
    "Finalizado" boolean NOT NULL,
    CONSTRAINT "PK_Partidos" PRIMARY KEY ("Id")
);

CREATE TABLE "Predicciones" (
    "Id" uuid NOT NULL,
    "UsuarioId" uuid NOT NULL,
    "PartidoId" uuid NOT NULL,
    "GolesLocalPrediccion" integer NOT NULL,
    "GolesVisitantePrediccion" integer NOT NULL,
    "PuntosGanados" integer NOT NULL,
    CONSTRAINT "PK_Predicciones" PRIMARY KEY ("Id")
);

CREATE TABLE "Usuarios" (
    "Id" uuid NOT NULL,
    "Nombre" character varying(50) NOT NULL,
    CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260521002215_InitialCreate', '6.0.29');

COMMIT;

START TRANSACTION;

ALTER TABLE "Partidos" DROP CONSTRAINT "PK_Partidos";

ALTER TABLE "Partidos" RENAME TO partidos;

ALTER TABLE partidos RENAME COLUMN "Finalizado" TO finalizado;

ALTER TABLE partidos RENAME COLUMN "Id" TO id;

ALTER TABLE partidos RENAME COLUMN "VisitanteId" TO visitante_id;

ALTER TABLE partidos RENAME COLUMN "LocalId" TO local_id;

ALTER TABLE partidos RENAME COLUMN "GolesVisitanteReal" TO goles_visitante_real;

ALTER TABLE partidos RENAME COLUMN "GolesLocalReal" TO goles_local_real;

ALTER TABLE partidos RENAME COLUMN "FechaId" TO fecha_id;

ALTER TABLE "Fechas" RENAME COLUMN "Nombre" TO nombre;

ALTER TABLE "Fechas" RENAME COLUMN "Id" TO id;

ALTER TABLE "Equipos" RENAME COLUMN "Nombre" TO nombre;

ALTER TABLE "Equipos" RENAME COLUMN "Id" TO id;

ALTER TABLE "Equipos" RENAME COLUMN "LogoUrl" TO logo_url;

ALTER TABLE "Usuarios" ADD "Email" text NOT NULL DEFAULT '';

ALTER TABLE "Usuarios" ADD "FechaRegistro" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "Usuarios" ADD "PasswordHash" text NOT NULL DEFAULT '';

ALTER TABLE "Usuarios" ADD "Username" text NOT NULL DEFAULT '';

ALTER TABLE "Equipos" ADD "Grupo" text NOT NULL DEFAULT '';

ALTER TABLE partidos ADD CONSTRAINT "PK_partidos" PRIMARY KEY (id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260529192412_AgregarGrupoAEquipos', '6.0.29');

COMMIT;

