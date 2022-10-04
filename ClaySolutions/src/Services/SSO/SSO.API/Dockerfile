#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/SSO/SSO.API/SSO.API.csproj", "src/Services/SSO/SSO.API/"]
COPY ["src/Services/SSO/SSO.Application/SSO.Application.csproj", "src/Services/SSO/SSO.Application/"]
COPY ["src/BuildingBlocks/SharedKernel/SharedKernel.csproj", "src/BuildingBlocks/SharedKernel/"]
COPY ["src/Services/SSO/SSO.Domain/SSO.Domain.csproj", "src/Services/SSO/SSO.Domain/"]
COPY ["src/Services/SSO/SSO.Infrastructure/SSO.Infrastructure.csproj", "src/Services/SSO/SSO.Infrastructure/"]
COPY ["src/BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "src/BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "src/Services/SSO/SSO.API/SSO.API.csproj"
COPY . .
WORKDIR "/src/src/Services/SSO/SSO.API"
RUN dotnet build "SSO.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSO.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSO.API.dll"]