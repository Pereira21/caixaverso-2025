Endpoints:
[POST] /api/Investimentos/simular-investimento -> Acesso público para todos os brasileiros simularem.
[GET] /api/Investimentos/simulacoes -> Acesso exige autenticação mínima. Visualização interna.
[GET] /api/Investimentos/simulacoes/por-produto-dia -> Acesso exige autenticação mínima. Visualização interna.
[GET] /api/Telemetria/telemetria -> Acesso exige autenticação de usuário com role admin. 

Usuários pré-cadastrados (via Program.cs):
E-mail: admin@admin.com / Senha: @Admin123   <- Usuário com role admin.
E-mail: usuario@teste.com / Senha: @User123  <- Usuário sem role para endpoints internos.



Tecnologias abordadas no projeto:
O projeto segue Clean Architecture e aplica um CQRS simples.
Em vez de um único repositório, separo os repositórios de escrita (commands) dos repositórios de leitura (queries).
As escritas trabalham apenas com entidades do domínio, e as leituras usam projeções e DTOs para otimizar performance.