#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001
EXPOSE 5101

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["../ICAP_MarketService/ICAP_MarketService.csproj", "ICAP_MarketService/"]
COPY ["../ICAP_Infrastructure/ICAP_Infrastructure.csproj", "ICAP_Infrastructure/"]
COPY ["../ICAP_ServiceBus/ICAP_ServiceBus.csproj", "ICAP_ServiceBus/"]
RUN dotnet restore "./ICAP_MarketService/./ICAP_MarketService.csproj"
COPY . .
WORKDIR "/src/ICAP_MarketService"
RUN dotnet build "./ICAP_MarketService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ICAP_MarketService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ICAP_MarketService.dll"]