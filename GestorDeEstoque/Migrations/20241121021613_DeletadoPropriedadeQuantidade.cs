using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class DeletadoPropriedadeQuantidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Estoques_EstoqueId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_EstoqueId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "EstoqueId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Produtos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "ProdutosEstoques",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosEstoques_Estoques_EstoqueId",
                table: "ProdutosEstoques");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosEstoques_Produtos_ProdutoId",
                table: "ProdutosEstoques");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutosEstoques",
                table: "ProdutosEstoques");

            migrationBuilder.RenameTable(
                name: "ProdutosEstoques",
                newName: "ProdutoEstoque");

            migrationBuilder.RenameIndex(
                name: "IX_ProdutosEstoques_EstoqueId",
                table: "ProdutoEstoque",
                newName: "IX_ProdutoEstoque_EstoqueId");

            migrationBuilder.AddColumn<int>(
                name: "EstoqueId",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantidade",
                table: "Produtos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "ProdutoEstoque",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutoEstoque",
                table: "ProdutoEstoque",
                columns: new[] { "ProdutoId", "EstoqueId" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_EstoqueId",
                table: "Produtos",
                column: "EstoqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoEstoque_Estoques_EstoqueId",
                table: "ProdutoEstoque",
                column: "EstoqueId",
                principalTable: "Estoques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoEstoque_Produtos_ProdutoId",
                table: "ProdutoEstoque",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Estoques_EstoqueId",
                table: "Produtos",
                column: "EstoqueId",
                principalTable: "Estoques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
