using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestimentosCaixa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v001InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogTelemetria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Endpoint = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TempoRespostaMs = table.Column<int>(type: "int", nullable: false),
                    Sucesso = table.Column<bool>(type: "bit", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTelemetria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPontuacaoFrequencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinQtd = table.Column<int>(type: "int", nullable: false),
                    MaxQtd = table.Column<int>(type: "int", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPontuacaoFrequencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPontuacaoVolume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinValor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxValor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPontuacaoVolume", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilRisco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilRisco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilClassificacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilRiscoId = table.Column<int>(type: "int", nullable: false),
                    MinPontuacao = table.Column<int>(type: "int", nullable: false),
                    MaxPontuacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilClassificacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilClassificacao_PerfilRisco_PerfilRiscoId",
                        column: x => x.PerfilRiscoId,
                        principalTable: "PerfilRisco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPontuacaoRisco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiscoId = table.Column<int>(type: "int", nullable: false),
                    PontosBase = table.Column<int>(type: "int", nullable: false),
                    Multiplicador = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    PontosMaximos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPontuacaoRisco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPontuacaoRisco_Risco_RiscoId",
                        column: x => x.RiscoId,
                        principalTable: "Risco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelPerfilRisco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilRiscoId = table.Column<int>(type: "int", nullable: false),
                    RiscoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelPerfilRisco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelPerfilRisco_PerfilRisco_PerfilRiscoId",
                        column: x => x.PerfilRiscoId,
                        principalTable: "PerfilRisco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelPerfilRisco_Risco_RiscoId",
                        column: x => x.RiscoId,
                        principalTable: "Risco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    RiscoId = table.Column<int>(type: "int", nullable: false),
                    Liquidez = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoProduto_Risco_RiscoId",
                        column: x => x.RiscoId,
                        principalTable: "Risco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoProdutoId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    RentabilidadeAnual = table.Column<decimal>(type: "DECIMAL(5,4)", nullable: false),
                    PrazoMinimoMeses = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_TipoProduto_TipoProdutoId",
                        column: x => x.TipoProdutoId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Investimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rentabilidade = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimento_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investimento_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Simulacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    ValorInvestido = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrazoMeses = table.Column<short>(type: "smallint", nullable: false),
                    RentabilidadeEfetiva = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    DataSimulacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simulacao_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Investimento_ClienteId",
                table: "Investimento",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Investimento_ProdutoId",
                table: "Investimento",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilClassificacao_PerfilRiscoId",
                table: "PerfilClassificacao",
                column: "PerfilRiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPontuacaoRisco_RiscoId",
                table: "PerfilPontuacaoRisco",
                column: "RiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_TipoProdutoId",
                table: "Produto",
                column: "TipoProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_RelPerfilRisco_PerfilRiscoId_RiscoId",
                table: "RelPerfilRisco",
                columns: new[] { "PerfilRiscoId", "RiscoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelPerfilRisco_RiscoId",
                table: "RelPerfilRisco",
                column: "RiscoId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulacao_ProdutoId",
                table: "Simulacao",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoProduto_RiscoId",
                table: "TipoProduto",
                column: "RiscoId");

            migrationBuilder.Sql(@" 
                INSERT INTO Risco (Nome, Descricao) VALUES 
                    ('Baixo', 'Perfil de risco baixo'), 
                    ('Médio', 'Perfil de risco médio'), 
                    ('Alto', 'Perfil de risco alto');

                INSERT INTO TipoProduto (Nome, RiscoId, Liquidez, Descricao) VALUES 
                    ('Poupança', 1, 'Diária', 'Conta poupança com liquidez diária e baixo risco'), 
                    ('CDB', 2, 'Mensal', 'Certificado de Depósito Bancário com liquidez mensal e risco moderado'), 
                    ('Ações', 3, 'Variável', 'Investimento em ações com alta volatilidade e maior risco');

                INSERT INTO Produto (TipoProdutoId, Nome, RentabilidadeAnual, PrazoMinimoMeses) VALUES
                    (1, 'Poupança Caixa', 0.0650, 0),
                    (1, 'Poupança Caixa 2', 0.0640, 0),
                    (2, 'CDB Caixa 100% CDI', 0.1180, 6),
                    (2, 'CDB Caixa 110% CDI', 0.1220, 12),
                    (2, 'CDB Liquidez Diária Caixa', 0.1150, 0),
                    (3, 'Ações Petrobras (PETR4)', 0.1800, 0),
                    (3, 'Ações Vale (VALE3)', 0.1750, 0),
                    (3, 'Ações Magazine Luiza (MGLU3)', 0.2500, 0),
                    (3, 'ETF BOVA11', 0.1300, 0),
                    (2, 'CDB Caixa 102% CDI', 0.1190, 3);
    
                INSERT INTO PerfilPontuacaoVolume (MinValor, MaxValor, Pontos) VALUES 
                    (0.01, 5000, 10), 
                    (5000.01, 50000.00, 20), 
                    (50000.01, 99999999.99, 30);

                INSERT INTO PerfilPontuacaoFrequencia (MinQtd, MaxQtd, Pontos) VALUES 
                    (1, 2, 10), 
                    (3, 6, 20), 
                    (7, 99, 30);

                INSERT INTO PerfilPontuacaoRisco (RiscoId, PontosBase, Multiplicador, PontosMaximos) VALUES 
                    (1, 10, 1.5, 15),   -- Baixo risco → até 15
                    (2, 20, 1.25, 30),   -- Médio risco → até 25
                    (3, 25, 1.6, 45);   -- Alto risco → até 40

                INSERT INTO PerfilRisco (Nome, Descricao) VALUES 
                    ('Conservador', 'Perfil conservador com baixa tolerância ao risco'), 
                    ('Moderado', 'Perfil moderado com tolerância média ao risco'), 
                    ('Agressivo', 'Perfil agressivo com alta tolerância ao risco');

                INSERT INTO PerfilClassificacao (PerfilRiscoId, MinPontuacao, MaxPontuacao) VALUES 
                    (1, 0, 50),     -- Conservador
                    (2, 51, 80),    -- Moderado
                    (3, 81, 100);   -- Agressivo

                INSERT INTO RelPerfilRisco (PerfilRiscoId, RiscoId) VALUES 
                    (1, 1),  -- Conservador associado a Baixo risco
                    (2, 1),  -- Moderado associado a Baixo risco
                    (2, 2),  -- Moderado associado a Médio risco
                    (3, 2),  -- Agressivo associado a Médio risco
                    (3, 3);  -- Agressivo associado a Alto risco

                INSERT INTO Cliente (Id, Nome) VALUES 
                    (1, 'Lucas Pereira'),
                    (2, 'Mariana Silva'),
                    (3, 'João Ferreira'),
                    (4, 'Ana Moreira'),
                    (5, 'Bruno Almeida'),
                    (123, 'Teste 1'),
                    (1234, 'Teste 2');

                INSERT INTO Simulacao (ClienteId, ProdutoId, ValorInvestido, ValorFinal, PrazoMeses, RentabilidadeEfetiva, DataSimulacao) VALUES
                    (1, 1, 1500.00, 1597.50, 12, 0.0650, '2025-01-11'),
                    (1, 3,  890.00,  994.02, 6,  0.1180, '2025-02-04'),

                    (2, 4, 3000.00, 3366.00, 12, 0.1220, '2025-03-09'),
                    (2, 6, 2000.00, 2360.00, 12, 0.1800, '2025-03-21'),

                    (3, 8, 1200.00, 1500.00, 12, 0.2500, '2025-03-31'),
                    (3, 9, 2500.00, 2825.00, 12, 0.1300, '2025-04-14'),

                    (4, 2,  900.00,  957.60, 12, 0.0640, '2025-01-24'),
                    (4, 5, 4000.00, 4460.00, 12, 0.1150, '2025-02-17'),

                    (5, 7, 3200.00, 3760.00, 12, 0.1750, '2025-03-27'),
                    (5,10, 2000.00, 2238.00, 3,  0.1190, '2025-04-04');

                INSERT INTO Investimento (ClienteId, ProdutoId, Valor, Rentabilidade, Data) VALUES
                    (1, 1, 1500.00, 0.0650, '2025-01-12'),
                    (1, 1, 890.00, 0.1180, '2025-02-05'),
                    (2, 4, 3000.00, 0.1220, '2025-03-10'),
                    (2, 4, 2000.00, 0.1800, '2025-03-22'),
                    (3, 8, 1200.00, 0.2500, '2025-04-01'),
                    (3, 9, 2500.00, 0.1300, '2025-04-15'),
                    (4, 2, 900.00, 0.0640, '2025-01-25'),
                    (4, 5, 4000.00, 0.1150, '2025-02-18'),
                    (5, 7, 3200.00, 0.1750, '2025-03-28'),
                    (5, 10, 2000.00, 0.1190, '2025-04-05'),
                    (5, 10, 3000.00, 0.1190, '2025-04-05'),
                    (123, 6, 1500.00, 0.1190, '2025-04-05'),
                    (123, 6, 1500.00, 0.1190, '2025-04-05'),
                    (1234, 6, 1500.00, 0.1190, '2025-04-05'),
                    (1234, 7, 1500.00, 0.1190, '2025-04-05'),
                    (1234, 7, 100.00, 0.1190, '2025-04-05');

                INSERT INTO LogTelemetria VALUES
                    ('telemetria', 'GET', 250, 1, '2025-10-18 12:00:00.1945291');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Investimento");

            migrationBuilder.DropTable(
                name: "LogTelemetria");

            migrationBuilder.DropTable(
                name: "PerfilClassificacao");

            migrationBuilder.DropTable(
                name: "PerfilPontuacaoFrequencia");

            migrationBuilder.DropTable(
                name: "PerfilPontuacaoRisco");

            migrationBuilder.DropTable(
                name: "PerfilPontuacaoVolume");

            migrationBuilder.DropTable(
                name: "RelPerfilRisco");

            migrationBuilder.DropTable(
                name: "Simulacao");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "PerfilRisco");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "TipoProduto");

            migrationBuilder.DropTable(
                name: "Risco");
        }
    }
}
