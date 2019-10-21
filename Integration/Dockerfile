FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /Integration

COPY Integration.csproj .
RUN dotnet restore

COPY . .
ENTRYPOINT ["dotnet", "test"]