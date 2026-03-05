FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app_build

COPY ["src/SecureAuthApp.Api/SecureAuthApp.Api.csproj", "src/SecureAuthApp.Api/"]
COPY ["src/SecureAuthApp.Application/SecureAuthApp.Application.csproj", "src/SecureAuthApp.Application/"]
COPY ["src/SecureAuthApp.Domain/SecureAuthApp.Domain.csproj", "src/SecureAuthApp.Domain/"]
COPY ["src/SecureAuthApp.Infrastructure/SecureAuthApp.Infrastructure.csproj", "src/SecureAuthApp.Infrastructure/"]

RUN dotnet restore "src/SecureAuthApp.Api/SecureAuthApp.Api.csproj"
COPY . .
WORKDIR "/app_build/src/SecureAuthApp.Api"
RUN dotnet publish "SecureAuthApp.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SecureAuthApp.Api.dll"]
