using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class MudarRelacionamentoEntreEstoqueEProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstoqueProduto",
                columns: table => new
                {
                    EstoquesId = table.Column<int>(type: "integer", nullable: false),
                    ProdutosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueProduto", x => new { x.EstoquesId, x.ProdutosId });
                    table.ForeignKey(
                        name: "FK_EstoqueProduto_Estoques_EstoquesId",
                        column: x => x.EstoquesId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoqueProduto_Produtos_ProdutosId",
                        column: x => x.ProdutosId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_EstoqueProduto_ProdutosId",
                table: "EstoqueProduto",
                column: "ProdutosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstoqueProduto");
        }
    }
}
