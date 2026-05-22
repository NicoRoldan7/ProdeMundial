FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar archivos de proyectos metiéndose en la carpeta real
COPY SistemaTorneo/TorneosFutbol.API/TorneosFutbol.API.csproj SistemaTorneo/TorneosFutbol.API/
COPY SistemaTorneo/TorneosFutbol.Infrastructure/TorneosFutbol.Infrastructure.csproj SistemaTorneo/TorneosFutbol.Infrastructure/
COPY SistemaTorneo/TorneosFutbol.Domain/TorneosFutbol.Domain.csproj SistemaTorneo/TorneosFutbol.Domain/
RUN dotnet restore SistemaTorneo/TorneosFutbol.API/TorneosFutbol.API.csproj

# Copiar todo el resto del código
COPY . .
WORKDIR /src/SistemaTorneo/TorneosFutbol.API
RUN dotnet publish TorneosFutbol.API.csproj -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TorneosFutbol.API.dll"]
