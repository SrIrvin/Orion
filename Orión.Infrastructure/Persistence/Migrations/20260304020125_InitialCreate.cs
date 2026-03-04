using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Orión.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoComponente",
                columns: table => new
                {
                    ID_Estado = table.Column<int>(type: "integer", nullable: false),
                    Descripcion_Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoComponente", x => x.ID_Estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoSolicitud",
                columns: table => new
                {
                    ID_EstadoSolicitud = table.Column<int>(type: "integer", nullable: false),
                    Descripcion_Estado = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoSolicitud", x => x.ID_EstadoSolicitud);
                });

            migrationBuilder.CreateTable(
                name: "NivelCritico",
                columns: table => new
                {
                    ID_NivelCritico = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelCritico", x => x.ID_NivelCritico);
                });

            migrationBuilder.CreateTable(
                name: "TipoComponente",
                columns: table => new
                {
                    ID_TipoComponente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nombre_Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoComponente", x => x.ID_TipoComponente);
                });

            migrationBuilder.CreateTable(
                name: "TipoMantenimiento",
                columns: table => new
                {
                    ID_TipoMantto = table.Column<int>(type: "integer", nullable: false),
                    Descripcion_Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMantenimiento", x => x.ID_TipoMantto);
                });

            migrationBuilder.CreateTable(
                name: "Turno",
                columns: table => new
                {
                    ID_Turno = table.Column<int>(type: "integer", nullable: false),
                    Descripcion_Turno = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turno", x => x.ID_Turno);
                });

            migrationBuilder.CreateTable(
                name: "Ubicacion",
                columns: table => new
                {
                    ID_Ubicacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Numero_Nave = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicacion", x => x.ID_Ubicacion);
                });

            migrationBuilder.CreateTable(
                name: "Tecnico",
                columns: table => new
                {
                    ID_Personal = table.Column<int>(type: "integer", nullable: false),
                    Nombre_Apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ID_Turno = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnico", x => x.ID_Personal);
                    table.ForeignKey(
                        name: "FK_Tecnico_Turno",
                        column: x => x.ID_Turno,
                        principalTable: "Turno",
                        principalColumn: "ID_Turno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maquinaria",
                columns: table => new
                {
                    ID_Maquinaria = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Nombre_Maquina = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Fecha_Instalacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ID_NivelCritico = table.Column<int>(type: "integer", nullable: false),
                    ID_Ubicacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinaria", x => x.ID_Maquinaria);
                    table.ForeignKey(
                        name: "FK_Maquinaria_NivelCritico",
                        column: x => x.ID_NivelCritico,
                        principalTable: "NivelCritico",
                        principalColumn: "ID_NivelCritico",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Maquinaria_Ubicacion",
                        column: x => x.ID_Ubicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "ID_Ubicacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Componente",
                columns: table => new
                {
                    ID_Componente = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nombre_Componente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Numero_Serie = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Especificaciones_Tecnicas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Fecha_Ultimo_Cambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ID_Maquinaria = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    ID_TipoComponente = table.Column<int>(type: "integer", nullable: false),
                    ID_Estado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Componente", x => x.ID_Componente);
                    table.ForeignKey(
                        name: "FK_Componente_EstadoComponente",
                        column: x => x.ID_Estado,
                        principalTable: "EstadoComponente",
                        principalColumn: "ID_Estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Componente_Maquinaria",
                        column: x => x.ID_Maquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "ID_Maquinaria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Componente_TipoComponente",
                        column: x => x.ID_TipoComponente,
                        principalTable: "TipoComponente",
                        principalColumn: "ID_TipoComponente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitud_Servicio",
                columns: table => new
                {
                    ID_SS = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Descripcion_Falla = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Fecha_Apertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fecha_Cierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ID_Maquinaria = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    ID_TipoMantto = table.Column<int>(type: "integer", nullable: false),
                    ID_Personal = table.Column<int>(type: "integer", nullable: true),
                    ID_EstadoSolicitud = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitud_Servicio", x => x.ID_SS);
                    table.ForeignKey(
                        name: "FK_Solicitud_EstadoSolicitud",
                        column: x => x.ID_EstadoSolicitud,
                        principalTable: "EstadoSolicitud",
                        principalColumn: "ID_EstadoSolicitud",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_Maquinaria",
                        column: x => x.ID_Maquinaria,
                        principalTable: "Maquinaria",
                        principalColumn: "ID_Maquinaria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitud_Tecnico",
                        column: x => x.ID_Personal,
                        principalTable: "Tecnico",
                        principalColumn: "ID_Personal");
                    table.ForeignKey(
                        name: "FK_Solicitud_TipoMantenimiento",
                        column: x => x.ID_TipoMantto,
                        principalTable: "TipoMantenimiento",
                        principalColumn: "ID_TipoMantto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ID_Estado",
                table: "Componente",
                column: "ID_Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ID_Maquinaria",
                table: "Componente",
                column: "ID_Maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ID_TipoComponente",
                table: "Componente",
                column: "ID_TipoComponente");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoComponente_Descripcion_Estado",
                table: "EstadoComponente",
                column: "Descripcion_Estado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSolicitud_Descripcion_Estado",
                table: "EstadoSolicitud",
                column: "Descripcion_Estado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_ID_NivelCritico",
                table: "Maquinaria",
                column: "ID_NivelCritico");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_ID_Ubicacion",
                table: "Maquinaria",
                column: "ID_Ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_NivelCritico_Descripcion",
                table: "NivelCritico",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Servicio_ID_EstadoSolicitud",
                table: "Solicitud_Servicio",
                column: "ID_EstadoSolicitud");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Servicio_ID_Maquinaria",
                table: "Solicitud_Servicio",
                column: "ID_Maquinaria");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Servicio_ID_Personal",
                table: "Solicitud_Servicio",
                column: "ID_Personal");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Servicio_ID_TipoMantto",
                table: "Solicitud_Servicio",
                column: "ID_TipoMantto");

            migrationBuilder.CreateIndex(
                name: "IX_Tecnico_ID_Turno",
                table: "Tecnico",
                column: "ID_Turno");

            migrationBuilder.CreateIndex(
                name: "IX_TipoComponente_Nombre_Tipo",
                table: "TipoComponente",
                column: "Nombre_Tipo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoMantenimiento_Descripcion_Tipo",
                table: "TipoMantenimiento",
                column: "Descripcion_Tipo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turno_Descripcion_Turno",
                table: "Turno",
                column: "Descripcion_Turno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_Numero_Nave",
                table: "Ubicacion",
                column: "Numero_Nave",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Componente");

            migrationBuilder.DropTable(
                name: "Solicitud_Servicio");

            migrationBuilder.DropTable(
                name: "EstadoComponente");

            migrationBuilder.DropTable(
                name: "TipoComponente");

            migrationBuilder.DropTable(
                name: "EstadoSolicitud");

            migrationBuilder.DropTable(
                name: "Maquinaria");

            migrationBuilder.DropTable(
                name: "Tecnico");

            migrationBuilder.DropTable(
                name: "TipoMantenimiento");

            migrationBuilder.DropTable(
                name: "NivelCritico");

            migrationBuilder.DropTable(
                name: "Ubicacion");

            migrationBuilder.DropTable(
                name: "Turno");
        }
    }
}
