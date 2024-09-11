using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaLogEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_LogEstoque_LogEstoqueId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_LogEstoqueId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "LogEstoqueId",
                table: "Produtos");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "LogEstoque",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LogEstoque_ProdutoId",
                table: "LogEstoque",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEstoque_Produtos_ProdutoId",
                table: "LogEstoque",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEstoque_Produtos_ProdutoId",
                table: "LogEstoque");

            migrationBuilder.DropIndex(
                name: "IX_LogEstoque_ProdutoId",
                table: "LogEstoque");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "LogEstoque");

            migrationBuilder.AddColumn<int>(
                name: "LogEstoqueId",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_LogEstoqueId",
                table: "Produtos",
                column: "LogEstoqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_LogEstoque_LogEstoqueId",
                table: "Produtos",
                column: "LogEstoqueId",
                principalTable: "LogEstoque",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
