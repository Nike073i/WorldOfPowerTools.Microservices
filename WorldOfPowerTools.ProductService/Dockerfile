FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10502

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorldOfPowerTools.ProductService/WorldOfPowerTools.ProductService.csproj", "WorldOfPowerTools.ProductService/"]
RUN dotnet restore "WorldOfPowerTools.ProductService/WorldOfPowerTools.ProductService.csproj"
COPY . .
WORKDIR "/src/WorldOfPowerTools.ProductService"
RUN dotnet build "WorldOfPowerTools.ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldOfPowerTools.ProductService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldOfPowerTools.ProductService.dll"]