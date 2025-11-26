# Etapa base: runtime

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa build: SDK

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia todos os arquivos do projeto

COPY . .

# Restaura dependências e publica

RUN dotnet restore Projeto.csproj
RUN dotnet publish Projeto.csproj -c Release -o /app/publish

# Etapa final: runtime com arquivos publicados

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define o entrypoint da aplicação

ENTRYPOINT ["dotnet", "Projeto.dll"]
