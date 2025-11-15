CREATE DATABASE InvestimentosCaixa

USE InvestimentosCaixa;

INSERT INTO Risco (Nome, Descricao) VALUES 
('Baixo', 'Perfil de risco baixo'), 
('Médio', 'Perfil de risco médio'), 
('Alto', 'Perfil de risco alto');

INSERT INTO TipoProduto (Nome, RiscoId, Liquidez, Descricao) VALUES 
('Poupança', 1, 'Diária', 'Conta poupança com liquidez diária e baixo risco'), 
('CDB', 2, 'Mensal', 'Certificado de Depósito Bancário com liquidez mensal e risco moderado'), 
('Ações', 3, 'Variável', 'Investimento em ações com alta volatilidade e maior risco');

INSERT INTO Produto (TipoProdutoId, Nome, RentabilidadeAnual, PrazoMinimoMeses) VALUES 
(1, 'Poupança Caixa', 0.0400, 0), 
(2, 'CDB Caixa 12 meses', 0.1200, 12), 
(3, 'Ações XPTO', 0.2000, 6);
                
INSERT INTO PerfilPontuacaoVolume (MinValor, MaxValor, Pontos) VALUES 
(0.01, 5000.00, 10), 
(5000.01, 50000.00, 20), 
(50000.01, 99999999.99, 30);

INSERT INTO PerfilPontuacaoFrequencia (MinQtd, MaxQtd, Pontos) VALUES 
(1, 2, 10), 
(3, 6, 20), 
(7, 99, 30);

INSERT INTO PerfilPontuacaoRisco (RiscoId, PontosBase, Multiplicador, PontosMaximos) VALUES 
(1, 10, 1.0, 15),   -- Baixo risco ? até 15
(2, 20, 1.2, 30),   -- Médio risco ? até 30
(3, 30, 1.5, 45);   -- Alto risco ? até 45

INSERT INTO PerfilRisco (Nome, Descricao) VALUES 
('Conservador', 'Perfil conservador com baixa tolerância ao risco'), 
('Moderado', 'Perfil moderado com tolerância média ao risco'), 
('Agressivo', 'Perfil agressivo com alta tolerância ao risco');

INSERT INTO PerfilClassificacao (PerfilRiscoId, MinPontuacao, MaxPontuacao) VALUES 
(1, 0, 40),     -- Conservador
(2, 41, 75),    -- Moderado
(3, 76, 100);   -- Agressivo