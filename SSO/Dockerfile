# ====== Build (.NET 7) ======
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY SSO.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

# ====== Runtime (.NET 7) ======
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8081 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app ./

ENTRYPOINT ["dotnet", "SSO.dll"]

EXPOSE 8081
