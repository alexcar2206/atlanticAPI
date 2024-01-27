using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMJ_Backend.Migrations
{
    public partial class rellenarTablas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "asociaciones",
                columns: new[] { "Id_asociacion", "Descripcion", "Nombre", "Registration_date", "Ubicacion" },
                values: new object[,]
                {
                    { 1, "", "Asociacion 1", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4035), "" },
                    { 2, "", "Asociacion 2", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4064), "" },
                    { 3, "", "Asociacion 3", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4066), "" }
                });

            migrationBuilder.InsertData(
                table: "insignias",
                columns: new[] { "Id_insignia", "Descripcion", "Icon", "Nombre" },
                values: new object[,]
                {
                    { 1, "Insignia numero 1", new byte[0], "Insignia 1" },
                    { 2, "Insignia numero 2", new byte[0], "Insignia 2" },
                    { 3, "Insignia numero 3", new byte[0], "Insignia 3" }
                });

            migrationBuilder.InsertData(
                table: "intereses",
                columns: new[] { "Id_interes", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "", "Cine" },
                    { 2, "", "Música" },
                    { 3, "", "Juegos de mesa" },
                    { 4, "", "Electrónica" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id_rol", "Accesos", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "", "", "ADM" },
                    { 2, "", "", "JD" },
                    { 3, "", "", "JT" },
                    { 4, "", "", "SOC" },
                    { 5, "", "", "EXT" }
                });

            migrationBuilder.InsertData(
                table: "usuarias",
                columns: new[] { "Id_usuaria", "Apellidos", "Bio", "CodPostal", "Contra", "DNI", "Edad", "Email", "FechaNacimiento", "Fk_IdAsociacion", "Fk_idInsignia", "Fk_idRol", "Modification_date", "NickName", "Nombre", "Referentas", "Registration_date", "Socia", "TokenRecuperacionContrasena", "Ubicacion", "Validado", "Validation_date", "Validation_userid", "ValidezDNI","Condiciones" },
                values: new object[] { 1, "Martinez", "descripcion", 36209, "1234", "5385752N", 22, "sara22@gmail.com", new DateTime(2001, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4149), "Sara22", "Sara", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4147), true, "", "", true, new DateTime(1111, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),0 });

            migrationBuilder.InsertData(
                table: "usuarias",
                columns: new[] { "Id_usuaria", "Apellidos", "Bio", "CodPostal", "Contra", "DNI", "Edad", "Email", "FechaNacimiento", "Fk_IdAsociacion", "Fk_idInsignia", "Fk_idRol", "Modification_date", "NickName", "Nombre", "Referentas", "Registration_date", "Socia", "TokenRecuperacionContrasena", "Ubicacion", "Validado", "Validation_date", "Validation_userid", "ValidezDNI","Condiciones" },
                values: new object[] { 2, "Fernandez", "descripcion", 36209, "1234", "5385752N", 34, "Lucia34@gmail.com", new DateTime(1987, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 1, new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4157), "Lucia34", "Lucía", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4156), true, "", "", false, new DateTime(1111, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.InsertData(
                table: "fotos",
                columns: new[] { "Id_Foto", "Fk_idUsuaria", "Foto", "Tipo" },
                values: new object[,]
                {
                    { 1, 1, new byte[0], "PERFIL" },
                    { 2, 1, new byte[0], "DNI" },
                    { 4, 1, new byte[0], "PUBLICACION" },
                    { 5, 1, new byte[0], "PUBLICACION" },
                    { 6, 2, new byte[0], "PERFIL" },
                    { 7, 2, new byte[0], "DNI" },
                    { 9, 2, new byte[0], "PUBLICACION" },
                    { 10, 2, new byte[0], "PUBLICACION" }
                });

            migrationBuilder.InsertData(
                table: "interesesUsuarias",
                columns: new[] { "Id_InteresesUsuarias", "Fk_idInteres", "Fk_idUsuaria" },
                values: new object[,]
                {
                    { 1, 2, 1 },
                    { 2, 4, 1 },
                    { 3, 1, 2 },
                    { 4, 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "publicaciones",
                columns: new[] { "Id_publicacion", "Comentarios", "Cuerpo", "ModificacionDate", "Reacciones", "RegistrationDate", "fk_idFoto", "fk_idUsuaria" },
                values: new object[,]
                {
                    { 1, "", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4173), "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4171), 4, 1 },
                    { 2, "", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4178), "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4176), 5, 1 },
                    { 3, "", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4181), "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4180), 9, 2 },
                    { 4, "", "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4185), "", new DateTime(2023, 11, 20, 11, 52, 21, 328, DateTimeKind.Local).AddTicks(4184), 10, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "asociaciones",
                keyColumn: "Id_asociacion",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "asociaciones",
                keyColumn: "Id_asociacion",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "insignias",
                keyColumn: "Id_insignia",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "interesesUsuarias",
                keyColumn: "Id_InteresesUsuarias",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "interesesUsuarias",
                keyColumn: "Id_InteresesUsuarias",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "interesesUsuarias",
                keyColumn: "Id_InteresesUsuarias",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "interesesUsuarias",
                keyColumn: "Id_InteresesUsuarias",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "publicaciones",
                keyColumn: "Id_publicacion",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "publicaciones",
                keyColumn: "Id_publicacion",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "publicaciones",
                keyColumn: "Id_publicacion",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "publicaciones",
                keyColumn: "Id_publicacion",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id_rol",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id_rol",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id_rol",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id_rol",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "fotos",
                keyColumn: "Id_Foto",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "intereses",
                keyColumn: "Id_interes",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "intereses",
                keyColumn: "Id_interes",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "intereses",
                keyColumn: "Id_interes",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "intereses",
                keyColumn: "Id_interes",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "usuarias",
                keyColumn: "Id_usuaria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "usuarias",
                keyColumn: "Id_usuaria",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "asociaciones",
                keyColumn: "Id_asociacion",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "insignias",
                keyColumn: "Id_insignia",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "insignias",
                keyColumn: "Id_insignia",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id_rol",
                keyValue: 1);
        }
    }
}
