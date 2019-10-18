# Build Stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /API

COPY API.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -o /publish

# Runtime Image Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "API.dll"]