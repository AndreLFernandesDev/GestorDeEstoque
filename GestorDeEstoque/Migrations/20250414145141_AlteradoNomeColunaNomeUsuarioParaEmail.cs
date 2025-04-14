using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AlteradoNomeColunaNomeUsuarioParaEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "NomeUsuario", table: "Usuarios", newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_NomeUsuario",
                table: "Usuarios",
                newName: "IX_Usuarios_Email"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "Email", table: "Usuarios", newName: "NomeUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                newName: "IX_Usuarios_NomeUsuario"
            );
        }
    }
}
