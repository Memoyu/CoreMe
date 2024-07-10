#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CoreMe.Api/CoreMe.Api.csproj", "src/CoreMe.Api/"]
COPY ["src/CoreMe.Application/CoreMe.Application.csproj", "src/CoreMe.Application/"]
COPY ["src/CoreMe.Domain/CoreMe.Domain.csproj", "src/CoreMe.Domain/"]
COPY ["src/CoreMe.Infrastructure/CoreMe.Infrastructure.csproj", "src/CoreMe.Infrastructure/"]
RUN dotnet restore "./src/CoreMe.Api/CoreMe.Api.csproj"
COPY . .
WORKDIR "/src/src/CoreMe.Api"
RUN dotnet build "./CoreMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoreMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreMe.Api.dll"]