FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Commentium.API/Commentium.API.csproj", "Commentium.API/"]
COPY ["Commentium.Application/Commentium.Application.csproj", "Commentium.Application/"]
COPY ["Commentium.Domain/Commentium.Domain.csproj", "Commentium.Domain/"]
COPY ["Commentium.Persistence/Commentium.Persistence.csproj", "Commentium.Persistence/"]
RUN dotnet restore "./Commentium.API/Commentium.API.csproj"
COPY . .
WORKDIR "/src/Commentium.API"
RUN dotnet build "./Commentium.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Commentium.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Commentium.API.dll"]