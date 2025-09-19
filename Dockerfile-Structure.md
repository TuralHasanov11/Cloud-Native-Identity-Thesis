# Dockerfile Structure for .NET Services

This document explains the standard Dockerfile structure used for .NET services in this solution. The approach follows multi-stage builds for optimized, secure, and efficient container images.

---

## Overview

The Dockerfile is organized into multiple stages:
- **base**: Runtime environment for running the application
- **build**: Builds the application from source
- **publish**: Publishes the compiled application
- **final**: Produces the final image for deployment

---

## Stage Breakdown

### 1. `base` Stage
- **Purpose**: Provides the minimal runtime environment for .NET applications.
- **Image**: `mcr.microsoft.com/dotnet/aspnet:9.0`
- **Configuration**:
  - Sets a non-root user (`USER $APP_UID`) for security
  - Sets working directory to `/app`
  - Exposes required ports (e.g., `EXPOSE 5100`, `EXPOSE 5101`)

### 2. `build` Stage
- **Purpose**: Builds the .NET service from source code.
- **Image**: `mcr.microsoft.com/dotnet/sdk:9.0`
- **Configuration**:
  - Sets working directory to `/src`
  - Copies project and solution files
  - Restores NuGet packages
  - Builds the project using the specified configuration (default: Release)

### 3. `publish` Stage
- **Purpose**: Publishes the built application to a folder for deployment.
- **Configuration**:
  - Runs `dotnet publish` to generate optimized binaries
  - Output is placed in `/app/publish`

### 4. `final` Stage
- **Purpose**: Produces the final, production-ready image.
- **Image**: Uses the `base` stage for a minimal runtime
- **Configuration**:
  - Sets working directory to `/app`
  - Copies published output from the `publish` stage
  - Sets the entrypoint to run the application (e.g., `dotnet Basket.Api.dll`)

---

## Best Practices
- **Multi-stage builds**: Reduce image size and improve security by separating build and runtime environments.
- **Non-root user**: Always run containers as a non-root user for security.
- **Explicit port exposure**: Use `EXPOSE` to document which ports the service listens on.
- **Minimal runtime image**: Use the official ASP.NET runtime image for production.
- **No secrets in image**: Do not include sensitive data in the Dockerfile or image layers.

---

## Example Dockerfile Structure

```dockerfile
# Runtime base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5100
EXPOSE 5101

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Basket.Api/Basket.Api.csproj", "src/Basket.Api/"]
# ...other COPY commands...
RUN dotnet restore "./src/Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src/src/Basket.Api"
RUN dotnet build "./Basket.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "./Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]
```

---

## References
- [Microsoft Docs: Containerize a .NET app](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container)
- [Docker Official Documentation](https://docs.docker.com/engine/reference/builder/)

---

This structure ensures consistent, secure, and efficient Docker images for all .NET services in the solution.
