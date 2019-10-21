# Build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

WORKDIR /DeploymentPipelineTemplate

# Restore
COPY API/API.csproj ./API/
RUN dotnet restore API/API.csproj
COPY Tests/Tests.csproj ./Tests/
RUN dotnet restore Tests/Tests.csproj

# Copy src
COPY . .

# Test
ENV TEAMCITY_PROJECT_NAME=fake
RUN dotnet test Tests/Tests.csproj

# Publish
RUN dotnet publish API/API.csproj -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
COPY --from=build-env /publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "API.dll"]