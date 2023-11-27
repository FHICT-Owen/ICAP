#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ICAP_AccountService/ICAP_AccountService.csproj", "ICAP_AccountService/"]
COPY ["ICAP_Infrastructure/ICAP_Infrastructure.csproj", "ICAP_Infrastructure/"]
COPY ["ICAP_ServiceBus/ICAP_ServiceBus.csproj", "ICAP_ServiceBus/"]
RUN dotnet restore "./ICAP_AccountService/./ICAP_AccountService.csproj"
COPY . .
WORKDIR "/src/ICAP_AccountService"
RUN dotnet build "./ICAP_AccountService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ICAP_AccountService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ICAP_AccountService.dll"]