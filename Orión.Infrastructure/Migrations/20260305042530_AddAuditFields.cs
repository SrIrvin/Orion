using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orión.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Usuario",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tecnico",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tecnico",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Solicitud_Servicio",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Solicitud_Servicio",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Maquinaria",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Maquinaria",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Componente",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Componente",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tecnico");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tecnico");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Solicitud_Servicio");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Solicitud_Servicio");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Maquinaria");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Maquinaria");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Componente");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Componente");
        }
    }
}
