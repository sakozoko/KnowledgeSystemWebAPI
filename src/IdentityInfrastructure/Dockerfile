FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IdentityInfrastructure/IdentityInfrastructure.csproj", "src/IdentityInfrastructure/"]
RUN dotnet restore "src/IdentityInfrastructure/IdentityInfrastructure.csproj"
COPY . .
WORKDIR "/src/src/IdentityInfrastructure"
RUN dotnet build "IdentityInfrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityInfrastructure.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityInfrastructure.dll"]
