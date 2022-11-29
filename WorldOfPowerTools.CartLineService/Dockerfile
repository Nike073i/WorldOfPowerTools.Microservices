FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10503

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorldOfPowerTools.CartLineService/WorldOfPowerTools.CartService.csproj", "WorldOfPowerTools.CartLineService/"]
RUN dotnet restore "WorldOfPowerTools.CartLineService/WorldOfPowerTools.CartService.csproj"
COPY . .
WORKDIR "/src/WorldOfPowerTools.CartLineService"
RUN dotnet build "WorldOfPowerTools.CartService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldOfPowerTools.CartService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldOfPowerTools.CartService.dll"]