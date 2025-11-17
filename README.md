# 📌 API de Investimentos – Documentação de Endpoints

Este documento apresenta os endpoints disponíveis na API, suas finalidades, níveis de acesso e massa de testes.  
A arquitetura aplicada e o motor de recomendação também são descritos de forma clara e objetiva.

---

# 1. Endpoints + Explicações

## 🔐 1.1 Autenticação

### **[POST] /api/Auth/login**
**Acesso:** Público  
**Finalidade:** Atende a exigência de autenticação no sistema. Alguns endpoints são públicos, outros exigem token, e apenas o endpoint de telemetria requer *role* específica.

**Massa de Teste**  
- **Admin:**  
  - Email: `admin@admin.com`  
  - Senha: `@Admin123`
- **Usuário comum:**  
  - Email: `usuario@teste.com`  
  - Senha: `@User123`

---

## 📊 1.2 Perfil de Risco

### **[GET] /api/PerfisRisco/perfil-risco/{clienteId}**
**Acesso:** Público  
**Finalidade:** Retorna o Perfil de Risco do cliente por meio do motor de recomendação.

Como o desafio solicita um algoritmo simples baseado em:
- **volume de investimentos**, e  
- **frequência de movimentações**  

→ o motor prioriza **investimentos concretizados**.  
Caso o cliente **não tenha investimentos**, a análise recai sobre **simulações**, permitindo que o avaliador teste o motor sem depender de um endpoint de "investir".

### 🔧 Lógica do Motor de Recomendações

O score do cliente é calculado a partir de três componentes:

---

#### **1. Volume Total Investido**
A soma dos valores investidos é comparada à tabela `PerfilPontuacaoVolume`:

| Faixa (R$) | Pontos |
|------------|--------|
| 0,01 – 5.000,00 | 10 |
| 5.000,01 – 50.000,00 | 20 |
| 50.000,01 – 99.999.999,99 | 30 |

---

#### **2. Frequência de Movimentações**
Quantidade de movimentações → tabela `PerfilPontuacaoFrequencia`:

| Qtd. Movimentações | Pontos |
|--------------------|--------|
| 1 – 2 | 10 |
| 3 – 6 | 20 |
| 7 – 99 | 30 |

---

#### **3. Risco dos Produtos**
Com base na tabela `PerfilPontuacaoRisco`:

- Produtos são agrupados por risco (Baixo, Médio, Alto).
- Cada grupo recebe:
  - **PontosBase**
  - **Multiplicador** por quantidade
  - **PontosMáximos** como limite superior

Essa combinação captura diversidade + intensidade das escolhas de risco.

---

#### **4. Classificação Final**

| Score Final | Perfil |
|-------------|--------|
| 0 – 40 | Conservador |
| 41 – 75 | Moderado |
| 76 – 100 | Agressivo |

Esse processo garante análise objetiva e auditável.

---

## 🎯 1.3 Produtos Recomendados

### **[GET] /api/PerfisRisco/produtos-recomendados/{perfil}**
**Acesso:** Público  
**Finalidade:** Retorna os produtos recomendados com base no perfil informado.

**Massa de Teste** – tabela `PerfilRisco`:

```sql
INSERT INTO PerfilRisco (Nome, Descricao) VALUES 
('Conservador', 'Perfil conservador com baixa tolerância ao risco'), 
('Moderado', 'Perfil moderado com tolerância média ao risco'), 
('Agressivo', 'Perfil agressivo com alta tolerância ao risco');
```

---

## 💰 1.4 Investimentos

### **[GET] /api/Investimentos/investimentos/{clienteId}**
**Acesso:** Exige autenticação mínima  
**Finalidade:** Usuários internos podem visualizar os investimentos de um cliente.

**Massa de Teste** – tabela `PerfilRisco`:
```sql
INSERT INTO Investimento (ClienteId, ProdutoId, Valor, Rentabilidade, Data) VALUES
(1, 1, 1500.00, 0.0650, '2025-01-12'),
(1, 3, 890.00, 0.1180, '2025-02-05'),
(2, 4, 3000.00, 0.1220, '2025-03-10'),
(2, 6, 2000.00, 0.1800, '2025-03-22'),
(3, 8, 1200.00, 0.2500, '2025-04-01'),
(3, 9, 2500.00, 0.1300, '2025-04-15'),
(4, 2, 900.00, 0.0640, '2025-01-25'),
(4, 5, 4000.00, 0.1150, '2025-02-18'),
(5, 7, 3200.00, 0.1750, '2025-03-28'),
(5, 10, 2000.00, 0.1190, '2025-04-05');
```

---

## 🎯 1.5 Simulações

### **[POST] /api/Simulacoes/simular-investimento**
**Acesso:** Público
**Finalidade:** Permite simular investimentos.

**Massa de Teste** – N/A

---

### **[GET] /api/Simulacoes/simulacoes**
**Acesso:** Exige autenticação mínima
**Finalidade:** Usuários internos podem visualizar todas as simulações.

**Massa de Teste** – N/A

---

### **[GET] /api/Simulacoes/simulacoes/por-produto-dia**
**Acesso:** Exige autenticação mínima
**Finalidade:** Lista simulações agrupadas por produto e dia.

**Massa de Teste** – N/A

---

### **[GET] /api/Simulacoes/simulacoes/por-produto-dia**
**Acesso:** Exige autenticação mínima
**Finalidade:** Lista simulações agrupadas por produto e dia.

**Massa de Teste** – N/A

---

## 🎯 1.6 Telemetria

### **[GET] /api/Telemetrias/telemetria**
**Acesso:** Exige usuário com role admin
**Finalidade:** Endpoint técnico/gerencial para consultas internas.

**Massa de Teste** 
- Email: admin@admin.com
- Senha: @Admin123






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
