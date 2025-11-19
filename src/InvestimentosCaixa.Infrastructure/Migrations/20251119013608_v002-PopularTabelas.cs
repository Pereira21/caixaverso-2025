using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestimentosCaixa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v002PopularTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
                INSERT INTO Risco (Nome, Descricao) VALUES 
                    ('Baixo', 'Perfil de risco baixo'), 
                    ('Médio', 'Perfil de risco médio'), 
                    ('Alto', 'Perfil de risco alto');

                INSERT INTO TipoProduto (Nome, RiscoId, Liquidez, Descricao) VALUES 
                    ('Poupança', 1, 'Diária', 'Conta poupança com liquidez diária e baixo risco'), 
                    ('CDB', 2, 'Mensal', 'Certificado de Depósito Bancário com liquidez mensal e risco moderado'), 
                    ('Ações', 3, 'Variável', 'Investimento em ações com alta volatilidade e maior risco'),
                    ('LCI', 1, 'Mensal', 'Letra de Crédito Imobiliário isenta de IR para pessoa física e com baixo risco'),
                    ('LCA', 1, 'Mensal', 'Letra de Crédito do Agronegócio isenta de IR para pessoa física e com baixo risco'),
                    ('Tesouro Direto', 1, 'Diária', 'Títulos públicos federais considerados os investimentos mais seguros do país'),
                    ('Fundos', 2, 'Diária', 'Fundos de investimento com gestão profissional e risco variado conforme a estratégia');

                INSERT INTO Produto (TipoProdutoId, Nome, RentabilidadeAnual, PrazoMinimoMeses) VALUES
                    (1, 'Poupança Caixa', 0.0650, 0),
                    (1, 'Poupança Caixa 2', 0.0640, 0),
                    (2, 'CDB Caixa 100% CDI', 0.1180, 6),
                    (2, 'CDB Caixa 110% CDI', 0.1220, 12),
                    (2, 'CDB Liquidez Diária Caixa', 0.1150, 0),
                    (3, 'Ações Petrobras (PETR4)', 0.1800, 0),
                    (3, 'Ações Vale (VALE3)', 0.1750, 0),
                    (3, 'Ações Magazine Luiza (MGLU3)', 0.2500, 0),
                    (4, 'LCI Caixa Imobiliária 2027', 0.0950, 12),
                    (5, 'LCA Caixa Agronegócio 2026', 0.0920, 6),
                    (6, 'Tesouro IPCA+ 2029', 0.0650, 0),
                    (7, 'Fundo Multimercado Caixa Premium', 0.1100, 0);
    
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
                    (3, 81, 150);   -- Agressivo

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
                    (1, 1, 890.00, 0.0650, '2025-02-05'),
                    (2, 4, 3000.00, 0.1220, '2025-03-10'),
                    (2, 4, 2000.00, 0.1220, '2025-03-22'),
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
            migrationBuilder.Sql(@" 
                DELETE FROM LogTelemetria;
                DELETE FROM Investimento;
                DELETE FROM Simulacao;
                DELETE FROM Cliente;
                DELETE FROM RelPerfilRisco;
                DELETE FROM PerfilClassificacao;
                DELETE FROM PerfilRisco;
                DELETE FROM PerfilPontuacaoRisco;
                DELETE FROM PerfilPontuacaoFrequencia;
                DELETE FROM PerfilPontuacaoVolume;
                DELETE FROM Produto;
                DELETE FROM TipoProduto;
                DELETE FROM Risco;
            ");
        }
    }
}
