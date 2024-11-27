﻿// <auto-generated />
using System;
using GestorDeEstoque.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestorDeEstoque.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241126154428_AdicionarPropriedadeIdEstoqueATabelaLogEstoque")]
    partial class AdicionarPropriedadeIdEstoqueATabelaLogEstoque
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GestorDeEstoque.Models.Estoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Estoques");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.LogEstoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdEstoque")
                        .HasColumnType("integer");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("LogsEstoques");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.ProdutoEstoque", b =>
                {
                    b.Property<int>("ProdutoId")
                        .HasColumnType("integer");

                    b.Property<int>("EstoqueId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("numeric");

                    b.HasKey("ProdutoId", "EstoqueId");

                    b.HasIndex("EstoqueId");

                    b.ToTable("ProdutosEstoques");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.LogEstoque", b =>
                {
                    b.HasOne("GestorDeEstoque.Models.Produto", "Produto")
                        .WithMany("LogsEstoque")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.ProdutoEstoque", b =>
                {
                    b.HasOne("GestorDeEstoque.Models.Estoque", "Estoque")
                        .WithMany("ProdutosEstoques")
                        .HasForeignKey("EstoqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestorDeEstoque.Models.Produto", "Produto")
                        .WithMany("ProdutosEstoques")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estoque");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.Estoque", b =>
                {
                    b.Navigation("ProdutosEstoques");
                });

            modelBuilder.Entity("GestorDeEstoque.Models.Produto", b =>
                {
                    b.Navigation("LogsEstoque");

                    b.Navigation("ProdutosEstoques");
                });
#pragma warning restore 612, 618
        }
    }
}
