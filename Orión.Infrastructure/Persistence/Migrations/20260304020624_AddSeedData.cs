using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Orión.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstadoComponente",
                columns: new[] { "ID_Estado", "Descripcion_Estado" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "En reparacion" },
                    { 3, "Dado de baja" },
                    { 4, "En stock" }
                });

            migrationBuilder.InsertData(
                table: "EstadoSolicitud",
                columns: new[] { "ID_EstadoSolicitud", "Descripcion_Estado" },
                values: new object[,]
                {
                    { 1, "Abierta" },
                    { 2, "En proceso" },
                    { 3, "Esperando proveedor" },
                    { 4, "Finalizada" },
                    { 5, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "NivelCritico",
                columns: new[] { "ID_NivelCritico", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Baja" },
                    { 2, "Media" },
                    { 3, "Alta" },
                    { 4, "Critico" }
                });

            migrationBuilder.InsertData(
                table: "TipoMantenimiento",
                columns: new[] { "ID_TipoMantto", "Descripcion_Tipo" },
                values: new object[,]
                {
                    { 1, "Preventivo" },
                    { 2, "Correctivo" }
                });

            migrationBuilder.InsertData(
                table: "Turno",
                columns: new[] { "ID_Turno", "Descripcion_Turno" },
                values: new object[,]
                {
                    { 1, "Matutino" },
                    { 2, "Vespertino" },
                    { 3, "Nocturno" },
                    { 4, "Mixto" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstadoComponente",
                keyColumn: "ID_Estado",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadoComponente",
                keyColumn: "ID_Estado",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadoComponente",
                keyColumn: "ID_Estado",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstadoComponente",
                keyColumn: "ID_Estado",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EstadoSolicitud",
                keyColumn: "ID_EstadoSolicitud",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadoSolicitud",
                keyColumn: "ID_EstadoSolicitud",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadoSolicitud",
                keyColumn: "ID_EstadoSolicitud",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstadoSolicitud",
                keyColumn: "ID_EstadoSolicitud",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EstadoSolicitud",
                keyColumn: "ID_EstadoSolicitud",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NivelCritico",
                keyColumn: "ID_NivelCritico",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NivelCritico",
                keyColumn: "ID_NivelCritico",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NivelCritico",
                keyColumn: "ID_NivelCritico",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NivelCritico",
                keyColumn: "ID_NivelCritico",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TipoMantenimiento",
                keyColumn: "ID_TipoMantto",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoMantenimiento",
                keyColumn: "ID_TipoMantto",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Turno",
                keyColumn: "ID_Turno",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Turno",
                keyColumn: "ID_Turno",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Turno",
                keyColumn: "ID_Turno",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Turno",
                keyColumn: "ID_Turno",
                keyValue: 4);
        }
    }
}
