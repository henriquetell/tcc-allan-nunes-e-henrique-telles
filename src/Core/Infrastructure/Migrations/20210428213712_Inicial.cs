using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conteudo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(max)", nullable: false),
                    Assunto = table.Column<string>(type: "varchar(500)", nullable: true),
                    IdConteudo = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conteudo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrupoUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descricao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    NomeGrupo = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConteudoAnexo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdConteudo = table.Column<int>(nullable: false),
                    Anexo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    NomeOriginal = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoAnexo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConteudoAnexo_Conteudo_IdConteudo",
                        column: x => x.IdConteudo,
                        principalTable: "Conteudo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    Codigo = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    Imagem = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: true),
                    DescricaoLonga = table.Column<string>(type: "varchar(max)", nullable: true),
                    IdConteudo = table.Column<int>(nullable: false),
                    CategoriaProduto = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    TotalPromotores = table.Column<int>(nullable: true),
                    TotalDetratores = table.Column<int>(nullable: true),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Conteudo_IdConteudo",
                        column: x => x.IdConteudo,
                        principalTable: "Conteudo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Imagem = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    Senha = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    Salt = table.Column<byte[]>(maxLength: 500, nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    IdGrupoUsuario = table.Column<int>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_GrupoUsuario_IdGrupoUsuario",
                        column: x => x.IdGrupoUsuario,
                        principalTable: "GrupoUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissaoAcao",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TipoAcao = table.Column<int>(nullable: false),
                    IdPermissao = table.Column<Guid>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissaoAcao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissaoAcao_Permissao_IdPermissao",
                        column: x => x.IdPermissao,
                        principalTable: "Permissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoNps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Comentario = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    DataResposta = table.Column<DateTime>(nullable: true),
                    DataEnvio = table.Column<DateTime>(nullable: true),
                    DataLimite = table.Column<DateTime>(nullable: true),
                    Nota = table.Column<int>(nullable: true),
                    IdProduto = table.Column<int>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoNps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoNps_Produto_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrupoUsuarioPermisaoAcao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGrupoUsuario = table.Column<int>(nullable: false),
                    IdPermissaoAcao = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoUsuarioPermisaoAcao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoUsuarioPermisaoAcao_GrupoUsuario_IdGrupoUsuario",
                        column: x => x.IdGrupoUsuario,
                        principalTable: "GrupoUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GrupoUsuarioPermisaoAcao_PermissaoAcao_IdPermissaoAcao",
                        column: x => x.IdPermissaoAcao,
                        principalTable: "PermissaoAcao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdConteudo",
                table: "ConteudoAnexo",
                column: "IdConteudo");

            migrationBuilder.CreateIndex(
                name: "IX_IdGrupoUsuario",
                table: "GrupoUsuarioPermisaoAcao",
                column: "IdGrupoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_IdPermissaoAcao",
                table: "GrupoUsuarioPermisaoAcao",
                column: "IdPermissaoAcao");

            migrationBuilder.CreateIndex(
                name: "IX_IdPermissao",
                table: "PermissaoAcao",
                column: "IdPermissao");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_Codigo",
                table: "Produto",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdConteudo",
                table: "Produto",
                column: "IdConteudo");

            migrationBuilder.CreateIndex(
                name: "IX_IdProduto",
                table: "ProdutoNps",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_IdGrupoUsuario",
                table: "Usuario",
                column: "IdGrupoUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConteudoAnexo");

            migrationBuilder.DropTable(
                name: "GrupoUsuarioPermisaoAcao");

            migrationBuilder.DropTable(
                name: "ProdutoNps");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "PermissaoAcao");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "GrupoUsuario");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Conteudo");
        }
    }
}
