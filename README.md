<h1>1.0 Endpoints + Explicações</h1></br>
<h1>1.1 Massa de Testes</h1></br>
<h1>2.0 Arquitetura + Features</h1></br></br>

<h3>1.0 Endpoints + Explicações:</h3></br>
<h5>[POST] /api/Auth/login</h5></br>
<b>Acesso</b>: Público</br>
<b>Finalidade</b>: Atender exigência de uso de autenticação no sistema. Para demonstrar domínio do tema, alguns endpoints são públicos, outros exigem token, sendo telemetria a única a exigir 'role'.</br>
<b>Massa de teste</b>:</br>
E-mail: admin@admin.com / Senha: @Admin123   <- Usuário com role admin.</br>
E-mail: usuario@teste.com / Senha: @User123  <- Usuário sem role para endpoints internos.</br></br>

[GET]  /api/PerfisRisco/perfil-risco/{clienteId}
Acesso: Público para todos os brasileiros acessarem.

Finalidade: Através do motor de recomendação traçar o Perfil de Risco do Cliente. Como o desafio pede um algoritmo simples focado em 'vol. de <b>investimentos</b> e frequência de <b>movimentações</b> interpretei que o motor deve analisar os investimentos concretizados e não as simulações. Entretanto, como não há endpoint de investir, inseri a regra abaixo para caso queira, o avaliador possa gerar dados e testar o motor de recomendação:
Motor prioriza análise de investimentos do cliente. Caso o cliente informado não tenha investimentos, o motor analisará simulações.

<b>Massa de teste</b> de investimentos na sessão de massa de testes.

Lógica escolhida <b>Motor de Recomendações</b>:
Usando as movimentações ou simulações como base, o motor insere uma pontuação score para o cliente a partir de três parâmetros: Volume total investido, Frequência de movimentações e Risco dos Produtos movimentados. No fim, os três scores são somados para se ter um score final do cliente e esse score é usado para determinar qual o perfil correspondente. Todas as informações de pontuação estão parametrizadas no banco de dados como boa prática para deixar a alteração dos dados dinâmica.

1. Volume Total Investido
A soma dos valores investidos em todas as movimentações é usada para encontrar a faixa correspondente na tabela PerfilPontuacaoVolume. Faixas maiores de investimento contribuem com mais pontos.

2. Frequência de Movimentações
A quantidade de movimentações do cliente é comparada às faixas de PerfilPontuacaoFrequencia. Quanto mais simulações, maior a pontuação atribuída.

3. Risco dos Produtos Simulados
As movimentações são agrupadas pelo risco dos produtos (Baixo, Médio ou Alto), e para cada grupo é aplicado o seguinte cálculo:
- Atributo PontosBase define a pontuação inicial para cada tipo de risco.
- O Multiplicador aumenta a pontuação para múltiplas movimentações do mesmo risco.
- Existe ainda um teto máximo (PontosMaximos) para evitar pontuação desproporcional.

Esta combinação permite capturar tanto a diversidade quanto a intensidade das escolhas de risco do investidor. As faixas são encontradas na tabela PerfilPontuacaoRisco.

4. Classificação Final
Com a pontuação final consolidada, o sistema consulta a tabela PerfilClassificacao para determinar o perfil:
- 0 a 40 pontos → Conservador
- 41 a 75 pontos → Moderado
- 76 a 100 pontos → Agressivo

Esse processo garante uma análise consistente, transparente e baseada em critérios objetivos definidos pela instituição.

[GET]  /api/PerfisRisco/produtos-recomendados/{perfil} -> Acesso público para todos os brasileiros acessarem.

[GET]  /api/Investimentos/investimentos/{clienteId} -> Acesso exige autenticação mínima. Visualização interna.

[POST] /api/Simulacoes/simular-investimento -> Acesso público para todos os brasileiros simularem.

[GET]  /api/Simulacoes/simulacoes -> Acesso exige autenticação mínima. Visualização interna.

[GET]  /api/Simulacoes/simulacoes/por-produto-dia -> Acesso exige autenticação mínima. Visualização interna.

[GET]  /api/Telemetrias/telemetria -> Acesso exige autenticação de usuário com role admin. 


Usuários pré-cadastrados (via Program.cs):
E-mail: admin@admin.com / Senha: @Admin123   <- Usuário com role admin.
E-mail: usuario@teste.com / Senha: @User123  <- Usuário sem role para endpoints internos.





Motor Recomendar Produtos
1. Compatibilidade de risco
- Perfil Conservador → aceitar apenas produtos com risco = Baixo (RiscoId = 1).
- Perfil Moderado → aceitar Baixo e Médio (RiscoId IN (1,2)).
- Perfil Agressivo → aceitar Médio e Alto (RiscoId IN (2,3)).



dotnet-reportgenerator-globaltool
coverage-report/index.html Testes


Tecnologias abordadas no projeto:
O projeto segue Clean Architecture e aplica um CQRS simples.
Em vez de um único repositório, separo os repositórios de escrita (commands) dos repositórios de leitura (queries).
As escritas trabalham apenas com entidades do domínio, e as leituras usam projeções e DTOs para otimizar performance.

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
                    (1, 'Poupança Bradesco', 0.0640, 0),
                    (2, 'CDB Banco Inter 100% CDI', 0.1180, 6),
                    (2, 'CDB Santander 110% CDI', 0.1220, 12),
                    (2, 'CDB Liquidez Diária BTG', 0.1150, 0),
                    (3, 'Ações Petrobras (PETR4)', 0.1800, 0),
                    (3, 'Ações Vale (VALE3)', 0.1750, 0),
                    (3, 'Ações Magazine Luiza (MGLU3)', 0.2500, 0),
                    (3, 'ETF BOVA11', 0.1300, 0),
                    (2, 'CDB Banco do Brasil 102% CDI', 0.1190, 3);
    
                INSERT INTO PerfilPontuacaoVolume (MinValor, MaxValor, Pontos) VALUES 
                    (0.01, 5000.00, 10), 
                    (5000.01, 50000.00, 20), 
                    (50000.01, 99999999.99, 30);

                INSERT INTO PerfilPontuacaoFrequencia (MinQtd, MaxQtd, Pontos) VALUES 
                    (1, 2, 10), 
                    (3, 6, 20), 
                    (7, 99, 30);

                INSERT INTO PerfilPontuacaoRisco (RiscoId, PontosBase, Multiplicador, PontosMaximos) VALUES 
                    (1, 10, 1.0, 15),   -- Baixo risco → até 15
                    (2, 20, 1.2, 30),   -- Médio risco → até 30
                    (3, 30, 1.5, 45);   -- Alto risco → até 45

                INSERT INTO PerfilRisco (Nome, Descricao) VALUES 
                    ('Conservador', 'Perfil conservador com baixa tolerância ao risco'), 
                    ('Moderado', 'Perfil moderado com tolerância média ao risco'), 
                    ('Agressivo', 'Perfil agressivo com alta tolerância ao risco');

                INSERT INTO PerfilClassificacao (PerfilRiscoId, MinPontuacao, MaxPontuacao) VALUES 
                    (1, 0, 40),     -- Conservador
                    (2, 41, 75),    -- Moderado
                    (3, 76, 100);   -- Agressivo

                INSERT INTO RelPerfilRisco (PerfilRiscoId, RiscoId) VALUES 
                    (1, 1),  -- Conservador associado a Baixo risco
                    (2, 1),  -- Moderado associado a Baixo risco
                    (2, 2),  -- Moderado associado a Médio risco
                    (3, 2),  -- Agressivo associado a Médio risco
                    (3, 3);  -- Agressivo associado a Alto risco

                INSERT INTO Cliente (Nome) VALUES 
                    ('Lucas Pereira');
                    ('Mariana Silva');
                    ('João Ferreira');
                    ('Ana Moreira');
                    ('Bruno Almeida');

                INSERT INTO Investimento (ClienteId, ProdutoId, Valor, Rentabilidade, Data) VALUES
                    (1, 1, 1500.00, 0.0650, '2025-01-12'),
                    (1, 3, 5000.00, 0.1180, '2025-02-05'),
                    (2, 4, 3000.00, 0.1220, '2025-03-10'),
                    (2, 6, 2000.00, 0.1800, '2025-03-22'),
                    (3, 8, 1200.00, 0.2500, '2025-04-01'),
                    (3, 9, 2500.00, 0.1300, '2025-04-15'),
                    (4, 2, 900.00, 0.0640, '2025-01-25'),
                    (4, 5, 4000.00, 0.1150, '2025-02-18'),
                    (5, 7, 3200.00, 0.1750, '2025-03-28'),
                    (5, 10, 2000.00, 0.1190, '2025-04-05');
            ");