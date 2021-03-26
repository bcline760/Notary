#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV NOTARY_APP_KEY ""
ENV NOTARY_AUTH_TYPE ""
ENV NOTARY_DB_CONN_STR ""

# Notary LDAP settings
ENV NOTARY_LDAP_ADMIN_GROUP ""
ENV NOTARY_LDAP_CERT_ADMIN_GROUP ""
ENV NOTARY_LDAP_DOMAIN ""
ENV NOTARY_LDAP_SEARCH ""
ENV NOTARY_LDAP_SERVER ""

ENV NOTARY_SERVICE_ACCOUNT_USER ""
ENV NOTARY_SERVICE_ACCOUNT_PASS ""

ENV NOTARY_TOKEN_AUDIENCE ""
ENV NOTARY_TOKEN_ISSUER "Notary"

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["api/Notary.Api.csproj", "api/"]
COPY ["library/contract/Notary.Contract.csproj", "library/contract/"]
COPY ["library/core/Notary.csproj", "library/core/"]
COPY ["library/interface/Notary.Interface.csproj", "library/interface/"]
COPY ["library/service/Notary.Service.csproj", "library/service/"]
COPY ["library/data/Notary.Data.csproj", "library/data/"]
COPY ["sln/Notary.sln", "sln/"]
RUN dotnet restore "sln/Notary.sln"

# Copy source code and build
COPY ["api/.", "api/"]
COPY ["library/contract/.", "library/contract/"]
COPY ["library/core/.", "library/core/"]
COPY ["library/interface/.", "library/interface/"]
COPY ["library/service/.", "library/service/"]
COPY ["library/data/.", "library/data/"]
WORKDIR /src/sln
RUN dotnet build -c Release -v m --nologo --no-restore
RUN dotnet publish --no-build --nologo --no-restore -v m -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "Notary.Api.dll"]