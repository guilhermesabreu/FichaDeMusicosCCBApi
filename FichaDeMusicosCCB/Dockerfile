FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FichaDeMusicosCCB.Api/FichaDeMusicosCCB.Api.csproj", "FichaDeMusicosCCB.Api/"]
COPY ["FichaDeMusicosCCB.Application/FichaDeMusicosCCB.Application.csproj", "FichaDeMusicosCCB.Application/"]
COPY ["FichaDeMusicosCCB.Domain/FichaDeMusicosCCB.Domain.csproj", "FichaDeMusicosCCB.Domain/"]
COPY ["FichaDeMusicosCCB.Persistence/FichaDeMusicosCCB.Persistence.csproj", "FichaDeMusicosCCB.Persistence/"]
RUN dotnet restore "FichaDeMusicosCCB.Api/FichaDeMusicosCCB.Api.csproj"
COPY . .
WORKDIR "/src/FichaDeMusicosCCB.Api"
RUN dotnet build "FichaDeMusicosCCB.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FichaDeMusicosCCB.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FichaDeMusicosCCB.Api.dll"]

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf