# Use Red Hat UBI .NET 8 base image for better OpenShift compatibility
FROM registry.access.redhat.com/ubi8/dotnet-80 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["pizza-api/pizza-api.csproj", "pizza-api/"]
RUN dotnet restore "./pizza-api/pizza-api.csproj"
COPY . .
WORKDIR "/src/pizza-api"
RUN dotnet build "./pizza-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./pizza-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Adjust permissions to allow OpenShift's default user to access the /app directory
RUN chown -R 1001:0 /app && chmod -R g+rwX /app

ENTRYPOINT ["dotnet", "pizza-api.dll"]
