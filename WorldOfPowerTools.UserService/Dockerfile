FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10501

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorldOfPowerTools.UserService/WorldOfPowerTools.UserService.csproj", "WorldOfPowerTools.UserService/"]
RUN dotnet restore "WorldOfPowerTools.UserService/WorldOfPowerTools.UserService.csproj"
COPY . .
WORKDIR "/src/WorldOfPowerTools.UserService"
RUN dotnet build "WorldOfPowerTools.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldOfPowerTools.UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldOfPowerTools.UserService.dll"]