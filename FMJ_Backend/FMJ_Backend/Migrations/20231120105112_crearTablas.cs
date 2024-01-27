using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMJ_Backend.Migrations
{
    public partial class crearTablas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asociaciones",
                columns: table => new
                {
                    Id_asociacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registration_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asociaciones", x => x.Id_asociacion);
                });

            migrationBuilder.CreateTable(
                name: "insignias",
                columns: table => new
                {
                    Id_insignia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insignias", x => x.Id_insignia);
                });

            migrationBuilder.CreateTable(
                name: "intereses",
                columns: table => new
                {
                    Id_interes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intereses", x => x.Id_interes);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accesos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id_rol);
                });

            migrationBuilder.CreateTable(
                name: "usuarias",
                columns: table => new
                {
                    Id_usuaria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenRecuperacionContrasena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidezDNI = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CodPostal = table.Column<int>(type: "int", nullable: false),
                    Socia = table.Column<bool>(type: "bit", nullable: false),
                    Fk_IdAsociacion = table.Column<int>(type: "int", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modification_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Validation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Condiciones = table.Column<bool>(type: "bit", nullable: false),
                    Validado = table.Column<bool>(type: "bit", nullable: false),
                    Validation_userid = table.Column<int>(type: "int", nullable: true),
                    Referentas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_idInsignia = table.Column<int>(type: "int", nullable: true),
                    Fk_idRol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarias", x => x.Id_usuaria);
                    table.ForeignKey(
                        name: "FK_usuarias_asociaciones_Fk_IdAsociacion",
                        column: x => x.Fk_IdAsociacion,
                        principalTable: "asociaciones",
                        principalColumn: "Id_asociacion");
                    table.ForeignKey(
                        name: "FK_usuarias_insignias_Fk_idInsignia",
                        column: x => x.Fk_idInsignia,
                        principalTable: "insignias",
                        principalColumn: "Id_insignia");
                    table.ForeignKey(
                        name: "FK_usuarias_roles_Fk_idRol",
                        column: x => x.Fk_idRol,
                        principalTable: "roles",
                        principalColumn: "Id_rol");
                });

            migrationBuilder.CreateTable(
                name: "fotos",
                columns: table => new
                {
                    Id_Foto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "varbinary(max)", nullable: false),
                    Fk_idUsuaria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fotos", x => x.Id_Foto);
                    table.ForeignKey(
                        name: "FK_fotos_usuarias_Fk_idUsuaria",
                        column: x => x.Fk_idUsuaria,
                        principalTable: "usuarias",
                        principalColumn: "Id_usuaria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interesesUsuarias",
                columns: table => new
                {
                    Id_InteresesUsuarias = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_idInteres = table.Column<int>(type: "int", nullable: false),
                    Fk_idUsuaria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interesesUsuarias", x => x.Id_InteresesUsuarias);
                    table.ForeignKey(
                        name: "FK_interesesUsuarias_intereses_Fk_idInteres",
                        column: x => x.Fk_idInteres,
                        principalTable: "intereses",
                        principalColumn: "Id_interes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_interesesUsuarias_usuarias_Fk_idUsuaria",
                        column: x => x.Fk_idUsuaria,
                        principalTable: "usuarias",
                        principalColumn: "Id_usuaria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "publicaciones",
                columns: table => new
                {
                    Id_publicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cuerpo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificacionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reacciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fk_idFoto = table.Column<int>(type: "int", nullable: false),
                    fk_idUsuaria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicaciones", x => x.Id_publicacion);
                    table.ForeignKey(
                        name: "FK_publicaciones_fotos_fk_idFoto",
                        column: x => x.fk_idFoto,
                        principalTable: "fotos",
                        principalColumn: "Id_Foto",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_publicaciones_usuarias_fk_idUsuaria",
                        column: x => x.fk_idUsuaria,
                        principalTable: "usuarias",
                        principalColumn: "Id_usuaria",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fotos_Fk_idUsuaria",
                table: "fotos",
                column: "Fk_idUsuaria");

            migrationBuilder.CreateIndex(
                name: "IX_interesesUsuarias_Fk_idInteres",
                table: "interesesUsuarias",
                column: "Fk_idInteres");

            migrationBuilder.CreateIndex(
                name: "IX_interesesUsuarias_Fk_idUsuaria",
                table: "interesesUsuarias",
                column: "Fk_idUsuaria");

            migrationBuilder.CreateIndex(
                name: "IX_publicaciones_fk_idFoto",
                table: "publicaciones",
                column: "fk_idFoto");

            migrationBuilder.CreateIndex(
                name: "IX_publicaciones_fk_idUsuaria",
                table: "publicaciones",
                column: "fk_idUsuaria");

            migrationBuilder.CreateIndex(
                name: "IX_usuarias_Fk_IdAsociacion",
                table: "usuarias",
                column: "Fk_IdAsociacion");

            migrationBuilder.CreateIndex(
                name: "IX_usuarias_Fk_idInsignia",
                table: "usuarias",
                column: "Fk_idInsignia");

            migrationBuilder.CreateIndex(
                name: "IX_usuarias_Fk_idRol",
                table: "usuarias",
                column: "Fk_idRol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interesesUsuarias");

            migrationBuilder.DropTable(
                name: "publicaciones");

            migrationBuilder.DropTable(
                name: "intereses");

            migrationBuilder.DropTable(
                name: "fotos");

            migrationBuilder.DropTable(
                name: "usuarias");

            migrationBuilder.DropTable(
                name: "asociaciones");

            migrationBuilder.DropTable(
                name: "insignias");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
