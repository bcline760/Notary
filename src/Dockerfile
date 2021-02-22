#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
ENV NOTARY_APP_KEY ""
ENV NOTARY_AUTH_TYPE "ActiveDirectory"
ENV NOTARY_DB_CONN_STR ""

# Notary LDAP settings
ENV NOTARY_LDAP_ADMIN_GROUP ""
ENV NOTARY_LDAP_CERT_ADMIN_GROUP ""
ENV NOTARY_LDAP_DOMAIN ""
ENV NOTARY_LDAP_SEARCH ""
ENV NOTARY_LDAP_SERVER ""

# Notary directories
ENV NOTARY_DIR_CA_ROOT "."
ENV NOTARY_DIR_INTERMEDIATE_CERTIFICATE "intermediate/certificates"
ENV NOTARY_DIR_INTERMEDIATE_PK "intermediate/keys"
ENV NOTARY_DIR_INTERMEDIATE_REQUEST "intermediate/requests"
ENV NOTARY_DIR_ISSUED_CERTIFICATE "issued/certificates"
ENV NOTARY_DIR_ISSUED_PK "issued/keys"
ENV NOTARY_DIR_ISSUED_REQUEST "issued/requests"
ENV NOTARY_DIR_ROOT_CERTIFICATE "root/certificates"
ENV NOTARY_DIR_ROOT_PK "root/keys"
ENV NOTARY_DIR_ROOT_REQUEST "root/requests"
ENV NOTARY_DIR_USER_KEY "user_keys"

ENV NOTARY_SERVICE_ACCOUNT_USER ""
ENV NOTARY_SERVICE_ACCOUNT_PASS ""

ENV NOTARY_TOKEN_AUDIENCE ""
ENV NOTARY_TOKEN_ISSUER "Notary"

RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

WORKDIR $NOTARY_CA_ROOT_DIR


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["api/Notary.Api.csproj", "src/api/Notary.Contract/"]
COPY ["library/contract/Notary.Contract.csproj", "src/library/contract/"]
COPY ["library/core/Notary.csproj", "src/Notary/"]
COPY ["library/interface/Notary.Interface.csproj", "src/library/interface/"]
COPY ["library/service/Notary.Service.csproj", "src/library/service/"]
COPY ["library/data/Notary.Data.csproj", "src/library/data/"]
RUN dotnet restore "src/api/Notary.Api.csproj"
COPY . .
WORKDIR "/src/api"
RUN dotnet build "Notary.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notary.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
EXPOSE 443
ENTRYPOINT ["dotnet", "Notary.Api.dll"]