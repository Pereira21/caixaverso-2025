# 📌 API de Investimentos – Documentação de Endpoints

Abaixo segue algumas particularidades nos endpoints que serão de interesse do(s) avaliador(es).

---

# 1. Endpoints + Explicações

## 1.1 Autenticação

### **[POST] /api/Auth/login**
**Acesso:** Público  
**Finalidade:** Atende a exigência de autenticação no sistema. Alguns endpoints são públicos, outros exigem token. Foram pré-cadastrados dois usuários com roles específicas que são exigidas nos endpoints privados. Inserido no swagger um endpoint para obter os usuários.

**Massa de Teste** 
- **Técnico:**  Para endpoints técnicos. Ex: Telemetria  
  - Email: `usuario@tecnico.com`  
  - Senha: `@Tecnico123`
- **Analista:** Para endpoints de análise interna. Ex: Listar investimentos por cliente, listar todas simulações, etc.  
  - Email: `usuario@analista.com`  
  - Senha: `@Analista123`

---

## 1.2 Perfil de Risco

### **[GET] /api/PerfisRisco/perfil-risco/{clienteId}**
**Acesso:** Público  
**Finalidade:** Retorna o Perfil de Risco do cliente por meio do motor de recomendação.

Como o desafio solicita um algoritmo simples baseado em:
- **volume de investimentos**, e  
- **frequência de movimentações**  

→ o motor prioriza **investimentos concretizados**.  
Caso o cliente **não tenha investimentos** realizados, a análise recai sobre **simulações**, permitindo que o avaliador teste o motor sem depender de um endpoint de "investir" (existe um de simular).

### 🔧 Lógica do Motor de Recomendações

O score do cliente é calculado a partir de três componentes:

---

#### **1. Volume Total Investido**
A soma dos valores investidos (ou simulados) é comparada à tabela `PerfilPontuacaoVolume`:

| Faixa (R$) | Pontos |
|------------|--------|
| 0,01 – 5.000,00 | 10 |
| 5.000,01 – 50.000,00 | 20 |
| 50.000,01 – 99.999.999,99 | 30 |

---

#### **2. Frequência de Movimentações**
Quantidade de movimentações (ou simulações) → tabela `PerfilPontuacaoFrequencia`:

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

| RiscoId | PontosBase | Multiplicador | PontosMaximos|
|---------|------------|---------------|--------------|
| 1 | 10 | 1.5 | 15 |
| 2 | 20 | 1.3 | 30 |
| 3 | 30 | 1.6 | 45 |

A pontuação foi dividida estratégicamente para respeitar as regras de mercado dando um peso um pouco maior ao fator risco, comparado à frequência e volume.
Essa combinação captura diversidade + intensidade das escolhas de risco.

Exemplo de caso: 
1. Cliente possui 3 investimentos de risco alto. Cálculo:
PontosBase +   1° inc     +     2° inc
30         + 30*(1.6 - 1) + (30*(1.6 - 1)) =
30         +       18     +     18  =
48 --ultrapassou limite de pontos maximos, então 45.  

2. Cliente possui 1 investimento de risco médio e 1 de risco baixo. Cálculo: Pontos Base Médio + inc risco baixo  
20 + 10*(1.5 - 1) = 20 + 5 = 25  

3. Cliente possui 2 investimentos de risco médio e 1 de risco baixo. Cálculo: Pontos Base Médio + inc risco médio + inc risco baixo  
20 + 20*(1.3 - 1) + 10*(1.5 - 1) =  
20 + 6 + 5 = 31  

Ps. Os pontos máximos são sempre aplicados encima de faixas do mesmo risco. Ou seja, se eu tivesse 4 médios e 1 baixo:  
( (20)  +  (20*(1.3 - 1))   +   (20*(1.3 - 1))   +   (20*(1.3 - 1)))   +   (10*(1.5 - 1)) =   
((20) + (5) + (5) + (5)) + (5) =  
(35) + 5 =  -- note que a soma dos riscos de nível médio ultrapassou o limite de 35, então o limite é aplicado: 35 -> 30
30 + 5 = 35  

Essa abordagem permite evitar furos nos pesos.

---

#### **4. Classificação Final**
Após os cálculos dos três tipos de scores, é feito uma soma entre os três e o resultado determina o Perfil do Cliente à partir da tabela abaixo:  

| Score Final | Perfil |
|-------------|--------|
| 0 – 50 | Conservador |
| 51 – 85 | Moderado |
| 86 – 150 | Agressivo |

Esse processo garante análise objetiva e auditável.

---

## 1.3 Produtos Recomendados

### **[GET] /api/PerfisRisco/produtos-recomendados/{perfil}**
**Acesso:** Público  
**Finalidade:** Retorna os produtos recomendados com base no perfil informado.

**Massa de Teste** – Os perfis pré-cadastrados estão na tabela `PerfilRisco`:

```sql
INSERT INTO PerfilRisco (Nome, Descricao) VALUES 
('Conservador', 'Perfil conservador com baixa tolerância ao risco'), 
('Moderado', 'Perfil moderado com tolerância média ao risco'), 
('Agressivo', 'Perfil agressivo com alta tolerância ao risco');
```

---

## 1.4 Investimentos

### **[GET] /api/Investimentos/investimentos/{clienteId}**
**Acesso:** Exige autenticação de um usuário com a role 'analista'  
**Finalidade:** Usuários com role 'analista' podem visualizar os investimentos de um cliente. 

---

## 1.5 Simulações

### **[POST] /api/Simulacoes/simular-investimento**  
**Acesso:** Público  
**Finalidade:** Permite simular investimentos.  
**Massa de Teste** – Incluído endpoint de listar produtos disponíveis. (MassaTeste)  

---

### **[GET] /api/Simulacoes/simulacoes**  
**Acesso:** Exige autenticação de um usuário com a role 'analista'  
**Finalidade:** Usuários internos com a role 'analista' podem visualizar todas as simulações.  

---

### **[GET] /api/Simulacoes/simulacoes/por-produto-dia**
**Acesso:** Exige autenticação de um usuário com a role 'analista'  
**Finalidade:** Lista simulações agrupadas por produto e dia.

**Massa de Teste** – Algumas simulações foram pré-cadastradas para que o avaliador tenha dados de dias diferentes.
```sql
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
```

---

## 1.6 Telemetria

### **[GET] /api/Telemetrias/telemetria**
**Acesso:** Exige usuário com role 'tecnico' por se tratar de um endpoint interno de análise técnica.  
**Finalidade:** Endpoint técnico/gerencial para consultas internas.  

**Massa de Teste**  – tabela `LogTelemetria`:
Como o endpoint agrupa por mês, o registro abaixo foi criado com um mês anterior para que se possa provar que o endpoint está cumprindo o papel.
```sql
INSERT INTO LogTelemetria VALUES
('telemetria', 'GET', 250, 1, '2025-10-18 12:00:00.1945291');
```
---

# 2. Arquitetura + Features

## 2.1 Arquitetura
API desenvolvida em .NET 8, estruturada seguindo os princípios de Clean Code. A solução é dividida em camadas independentes — API, Application, Domain, Infrastructure e Tests — garantindo **alta coesão interna** e **baixo acoplamento** entre os módulos. Essa organização permite evolução contínua do código, testabilidade aprimorada e separação clara entre regras de negócio, lógica de aplicação e detalhes de infraestrutura.

A aplicação utiliza **Entity Framework Core** como ORM e **SQL Server** como banco de dados principal, com suporte a testes utilizando a mesma base em ambiente controlado. Recursos como **Docker** e **Redis** são empregados para infraestrutura e otimização de performance, enquanto a documentação é exposta via **Swagger** para facilitar integração e inspeção dos endpoints.

## 2.2 Features
### Entity Framework Core
O projeto utiliza Entity Framework Core para abstrair o acesso ao SQL Server, permitindo trabalhar com o banco de forma tipada e organizada. O EF Core facilita o mapeamento das entidades, gerencia o rastreamento de alterações e oferece suporte completo a **Migrations**, garantindo evolução controlada do esquema.

### Redis
O projeto utiliza Redis como cache distribuído para otimizar consultas pouco voláteis. Algumas informações — como tabelas de scores e listas de produtos — são acessadas frequentemente, mas mudam raramente. Essas respostas são armazenadas no Redis para reduzir carga no banco, melhorar o tempo de resposta e garantir maior escalabilidade. 

### Swagger
A aplicação expõe documentação interativa via Swagger, permitindo visualizar e testar todos os endpoints da API. Ele gera a especificação OpenAPI automaticamente, facilitando integração com outros sistemas, garantindo transparência das rotas e acelerando o desenvolvimento e a validação das funcionalidades.

### Autenticaçaõ JWT
A aplicação utiliza JWT para garantir que apenas usuários autenticados possam acessar endpoints internos e sensíveis — como consultas de investimentos por cliente, listagem geral de simulações e rota de telemetria. O token carrega as credenciais e permissões do usuário, permitindo validação rápida. Essa abordagem reforça a segurança e mantém a arquitetura escalável.

### ASP.NET Identity
Utilizo ASP.NET Identity para gerenciar autenticação de forma estruturada e segura. Ele fornece todo o fluxo essencial — criação de usuários, hashing de senhas, validação de credenciais e gerenciamento de contas — reduzindo complexidade e garantindo boas práticas de segurança. A integração com JWT permite emitir tokens a partir do próprio sistema de identidade.

### Middleware de Exceção global
A API inclui um middleware de tratamento global de exceções para garantir respostas consistentes e seguras centralizadas. Ele captura erros não tratados, registra detalhes relevantes para diagnóstico e retorna mensagens padronizadas ao cliente, evitando vazamento de informações sensíveis e mantendo o comportamento uniforme da aplicação.

### AutoMapper
O projeto utiliza AutoMapper para simplificar a conversão entre entidades, DTOs e modelos de resposta. Ele reduz código repetitivo, centraliza regras de mapeamento e garante que a API exponha apenas os dados necessários.

### Rate Limiting (Segurança)
A API utiliza Rate Limiting nativo do ASP.NET Core para controlar a quantidade de requisições permitidas por cliente ou rota em um intervalo de tempo. Esse mecanismo protege o sistema contra abusos, garante uso equilibrado dos recursos e melhora a estabilidade da aplicação em cenários de alto tráfego. 

### Repository Pattern
O projeto aplica o Repository Pattern para isolar o acesso a dados e manter o domínio independente de detalhes de persistência. Cada repositório encapsula consultas e operações específicas, permitindo testar regras de negócio sem depender diretamente do banco. Essa abordagem deixa a aplicação mais modular, facilita manutenção e reforça os princípios da arquitetura limpa.

### Testes (Unitário e Integração)
O projeto conta com uma camada completa de **testes unitários e de integração**, garantindo a validação das regras de negócio e o correto funcionamento dos fluxos reais da API. Os testes unitários isolam comportamentos do domínio e da aplicação, enquanto os testes de integração verificam endpoints, repositórios, banco de dados e comportamentos end-to-end.
**A cobertura de código segue os padrões exigidos pela Caixa Econômica Federal, que requerem acima de 80%, e o projeto atualmente mantém cerca de 86% de cobertura total, garantindo alta confiabilidade e estabilidade.**

