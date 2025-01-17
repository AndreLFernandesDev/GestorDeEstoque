using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Preco = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsEstoques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProdutoId = table.Column<int>(type: "integer", nullable: false),
                    EstoqueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsEstoques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsEstoques_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogsEstoques_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosEstoques",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(type: "integer", nullable: false),
                    EstoqueId = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosEstoques", x => new { x.ProdutoId, x.EstoqueId });
                    table.ForeignKey(
                        name: "FK_ProdutosEstoques_Estoques_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosEstoques_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsEstoques_EstoqueId",
                table: "LogsEstoques",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsEstoques_ProdutoId",
                table: "LogsEstoques",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosEstoques_EstoqueId",
                table: "ProdutosEstoques",
                column: "EstoqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsEstoques");

            migrationBuilder.DropTable(
                name: "ProdutosEstoques");

            migrationBuilder.DropTable(
                name: "Estoques");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
