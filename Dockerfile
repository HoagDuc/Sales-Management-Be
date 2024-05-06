FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ptdn_net.csproj", "./"]
RUN dotnet restore "ptdn_net.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ptdn_net.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ptdn_net.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ptdn_net.dll"]
