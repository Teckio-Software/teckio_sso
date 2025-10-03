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

RUN [ -f /app/appsettings.Development.json ] || cp /app/appsettings.Template.json /app/appsettings.Development.json

RUN echo "Usando $( [ -f /app/appsettings.Development.json ] && echo appsettings.Development.json || echo appsettings.Template.json )"

ENTRYPOINT ["dotnet", "SSO.dll"]

EXPOSE 8081
