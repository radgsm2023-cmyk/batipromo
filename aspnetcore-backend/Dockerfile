# ==============================================================================
# Dockerfile Multi-Stage pour Backend ASP.NET Core 6 Web API
# ==============================================================================

# Étape 1 : Compilation du code C# avec le SDK .NET 6
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copie du fichier de projet et restauration des packages NuGet
COPY ["Batipromo.Api.csproj", "./"]
RUN dotnet restore "Batipromo.Api.csproj"

# Copie de l'ensemble du code source et publication en mode Release
COPY . .
RUN dotnet publish "Batipromo.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 2 : Image d'exécution légère ASP.NET 6
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Configuration des variables d'environnement
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

# Copie des binaires compilés
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Batipromo.Api.dll"]
