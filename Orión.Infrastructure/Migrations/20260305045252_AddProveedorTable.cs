using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Orión.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProveedorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProveedor",
                table: "Componente",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    ID_Proveedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RUC = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.ID_Proveedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Componente_IdProveedor",
                table: "Componente",
                column: "IdProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Componente_Proveedor",
                table: "Componente",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "ID_Proveedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Componente_Proveedor",
                table: "Componente");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Componente_IdProveedor",
                table: "Componente");

            migrationBuilder.DropColumn(
                name: "IdProveedor",
                table: "Componente");
        }
    }
}
