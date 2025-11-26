FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copia tudo
COPY . .

# restaura e publica usando o caminho correto
RUN dotnet restore PIM-Linkando-HTMLS-com-c-digos-C-\Projeto\Projeto.csproj
RUN dotnet publish PIM-Linkando-HTMLS-com-c-digos-C-\Projeto\Projeto.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Projeto.dll"]


