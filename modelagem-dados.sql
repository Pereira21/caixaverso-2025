CREATE DATABASE InvestimentosCaixa

USE InvestimentosCaixa;

CREATE TABLE TipoProduto (
    Id INTEGER PRIMARY KEY,
    Nome VARCHAR(50),            -- Ex.: "CDB", "Tesouro", "LCI", "Fundo"
    Risco VARCHAR(20),           -- Baixo, Médio, Alto
    Liquidez VARCHAR(20),        -- diária, mensal, vencimento
    Descricao VARCHAR(200)
);

INSERT INTO TipoProduto (Id, Nome, Risco, Liquidez, Descricao) VALUES
(1, 'CDB',      'Baixo',  'vencimento', 'Certificado de Depósito Bancário'),
(2, 'Tesouro',  'Baixo',  'diaria',     'Títulos públicos federais'),
(3, 'LCI',      'Médio',  'mensal',     'Letra de Crédito Imobiliário'),
(4, 'Fundo',    'Alto',   'mensal',     'Fundo multimercado');

CREATE TABLE Produto (
    Id INTEGER PRIMARY KEY,
    TipoProdutoId INTEGER NOT NULL,
    Nome VARCHAR(100),
    RentabilidadeAnual DECIMAL(5,4),
    PrazoMinimoMeses INT,
    FOREIGN KEY (TipoProdutoId) REFERENCES TipoProduto(Id)
);

INSERT INTO Produto VALUES
(101, 1, 'CDB Caixa 2026', 0.1200, 12),
(102, 1, 'CDB Liquidez Diária 2025', 0.1080, 1),
(103, 1, 'CDB Longo Prazo 2030', 0.1450, 36),
(104, 1, 'CDB Premium 2027', 0.1300, 18),
(105, 1, 'CDB Curto Prazo 6M', 0.0950, 6),
(201, 2, 'Tesouro Selic 2027', 0.1000, 1),
(202, 2, 'Tesouro IPCA+ 2035', 0.0600, 12),
(203, 2, 'Tesouro Prefixado 2029', 0.1150, 24),
(204, 2, 'Tesouro IPCA Curto 2029', 0.0550, 6),
(205, 2, 'Tesouro Prefixado Curto 2026', 0.1020, 3),
(301, 3, 'LCI Caixa 2026', 0.0920, 12),
(302, 3, 'LCA Agronegócio 2028', 0.1050, 24),
(303, 3, 'LCI Curto Prazo 9M', 0.0800, 9),
(304, 3, 'LCA Liquidez Mensal', 0.0870, 1),
(305, 3, 'LCI Alto Retorno 2030', 0.1250, 36),
(401, 4, 'Fundo Multimercado Dinâmico', 0.1650, 6),
(402, 4, 'Fundo Agressivo XPTO', 0.1800, 24),
(403, 4, 'Fundo Renda Fixa Premium', 0.1100, 1),
(404, 4, 'Fundo Ações Brasil', 0.2200, 12),
(405, 4, 'Fundo Moderado Equilíbrio', 0.1400, 3);

CREATE TABLE Simulacao (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    ProdutoId INT NOT NULL,
    ValorInvestido DECIMAL(18,2) NOT NULL,
    ValorFinal DECIMAL(18,2) NOT NULL,
    PrazoMeses INT NOT NULL,
    RentabilidadeEfetiva DECIMAL(10,4) NOT NULL,
    DataSimulacao DATETIME2 NOT NULL,

    FOREIGN KEY (ProdutoId) REFERENCES Produto(Id)
);

CREATE TABLE Telemetria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Endpoint VARCHAR(200) NOT NULL,
    Metodo VARCHAR(10) NOT NULL,
    TempoRespostaMs INT NOT NULL,
    Sucesso BIT NOT NULL,
    DataRegistro DATETIME NOT NULL
);