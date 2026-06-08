FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/NexusRoute.Api/NexusRoute.Api.csproj", "src/NexusRoute.Api/"]
COPY ["src/NexusRoute.Application/NexusRoute.Application.csproj", "src/NexusRoute.Application/"]
COPY ["src/NexusRoute.Domain/NexusRoute.Domain.csproj", "src/NexusRoute.Domain/"]
COPY ["src/NexusRoute.Infrastructure/NexusRoute.Infrastructure.csproj", "src/NexusRoute.Infrastructure/"]
COPY ["src/NexusRoute.Simulator/NexusRoute.Simulator.csproj", "src/NexusRoute.Simulator/"]

RUN dotnet restore "src/NexusRoute.Api/NexusRoute.Api.csproj"

COPY . .
WORKDIR "/src/src/NexusRoute.Api"
RUN dotnet build "NexusRoute.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NexusRoute.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NexusRoute.Api.dll"]
