#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/ClayService/ClayService.API/ClayService.API.csproj", "src/Services/ClayService/ClayService.API/"]
COPY ["src/Services/ClayService/ClayService.Application/ClayService.Application.csproj", "src/Services/ClayService/ClayService.Application/"]
COPY ["src/BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "src/BuildingBlocks/EventBus.Messages/"]
COPY ["src/BuildingBlocks/SharedKernel/SharedKernel.csproj", "src/BuildingBlocks/SharedKernel/"]
COPY ["src/Services/ClayService/ClayService.Domain/ClayService.Domain.csproj", "src/Services/ClayService/ClayService.Domain/"]
COPY ["src/Services/ClayService/ClayService.Infrastructure/ClayService.Infrastructure.csproj", "src/Services/ClayService/ClayService.Infrastructure/"]
RUN dotnet restore "src/Services/ClayService/ClayService.API/ClayService.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ClayService/ClayService.API"
RUN dotnet build "ClayService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClayService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClayService.API.dll"]