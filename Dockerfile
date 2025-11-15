# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia apenas o projeto da API
COPY src/InvestimentosCaixa.Api/InvestimentosCaixa.Api.csproj ./src/InvestimentosCaixa.Api/

# Copia também os outros projetos referenciados pela API
COPY src/InvestimentosCaixa.Domain/InvestimentosCaixa.Domain.csproj ./src/InvestimentosCaixa.Domain/
COPY src/InvestimentosCaixa.Application/InvestimentosCaixa.Application.csproj ./src/InvestimentosCaixa.Application/
COPY src/InvestimentosCaixa.Infrastructure/InvestimentosCaixa.Infrastructure.csproj ./src/InvestimentosCaixa.Infrastructure/

# Restaura dependências
RUN dotnet restore src/InvestimentosCaixa.Api/InvestimentosCaixa.Api.csproj

# Copia o resto do código
COPY . .

# Publica a API
WORKDIR /app/src/InvestimentosCaixa.Api/
RUN dotnet publish -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "InvestimentosCaixa.Api.dll"]
