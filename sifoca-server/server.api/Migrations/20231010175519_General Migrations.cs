using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.api.Migrations
{
    /// <inheritdoc />
    public partial class GeneralMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Fundo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Fundo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Movimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Area = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Caixa = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataRegistro = table.Column<string>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Movimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Entrada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Operador = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TipoPagamento = table.Column<string>(type: "TEXT", nullable: false),
                    Assinante = table.Column<string>(type: "TEXT", nullable: false),
                    DataRegistro = table.Column<string>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Entrada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Entrada_Tb_Movimento_Id",
                        column: x => x.Id,
                        principalTable: "Tb_Movimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Saida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Responsável = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DataRegistro = table.Column<string>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Saida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Saida_Tb_Movimento_Id",
                        column: x => x.Id,
                        principalTable: "Tb_Movimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tb_Fundo",
                columns: new[] { "Id", "Total" },
                values: new object[] { "caixa", 0m });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Fundo_Id",
                table: "Tb_Fundo",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Entrada");

            migrationBuilder.DropTable(
                name: "Tb_Fundo");

            migrationBuilder.DropTable(
                name: "Tb_Saida");

            migrationBuilder.DropTable(
                name: "Tb_Movimento");
        }
    }
}
