#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Notary.Web/Notary.Web.csproj", "src/Notary.Web/"]
COPY ["src/Notary.Contract/Notary.Contract.csproj", "src/Notary.Contract/"]
COPY ["src/Notary/Notary.csproj", "src/Notary/"]
COPY ["src/Notary.Interface/Notary.Interface.csproj", "src/Notary.Interface/"]
COPY ["src/Notary.Service/Notary.Service.csproj", "src/Notary.Service/"]
COPY ["src/Notary.Data/Notary.Data.csproj", "src/Notary.Data/"]
RUN dotnet restore "src/Notary.Web/Notary.Web.csproj"
COPY . .
WORKDIR "/src/Notary.Web"
RUN dotnet build "Notary.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notary.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
ENTRYPOINT ["dotnet", "Notary.Web.dll"]