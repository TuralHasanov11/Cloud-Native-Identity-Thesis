# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5108
EXPOSE 5109


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Webhooks.Api/Webhooks.Api.csproj", "src/Webhooks.Api/"]
COPY ["src/Audit/Audit.csproj", "src/Audit/"]
COPY ["src/ServiceDefaults/ServiceDefaults.csproj", "src/ServiceDefaults/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/Webhooks.Core/Webhooks.Core.csproj", "src/Webhooks.Core/"]
COPY ["src/Webhooks.Infrastructure/Webhooks.Infrastructure.csproj", "src/Webhooks.Infrastructure/"]
COPY ["src/Outbox/Outbox.csproj", "src/Outbox/"]
COPY ["src/EventBus/EventBus.csproj", "src/EventBus/"]
COPY ["src/Webhooks.Contracts/Webhooks.Contracts.csproj", "src/Webhooks.Contracts/"]
RUN dotnet restore "./src/Webhooks.Api/Webhooks.Api.csproj"
COPY . .
WORKDIR "/src/src/Webhooks.Api"
RUN dotnet build "./Webhooks.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Webhooks.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webhooks.Api.dll"]