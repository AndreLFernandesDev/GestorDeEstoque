using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class AlterarTipoDeMovimentacaoParaString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoDeMovimentacao",
                table: "LogsEstoques",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoDeMovimentacao",
                table: "LogsEstoques",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );
        }
    }
}
