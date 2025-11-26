# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo para dentro do container
COPY . .

# Lista a estrutura de pastas (opcional, só pra conferir)
RUN ls -R

# Restaura e publica qualquer projeto .csproj que estiver em subpastas
RUN dotnet restore */*.csproj
RUN dotnet publish */*.csproj -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Projeto.dll"]
