# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["BusData.csproj", "./"]
RUN dotnet restore "BusData.csproj"
COPY . .
RUN dotnet build "BusData.csproj" -c Release -o /app/build
RUN dotnet publish "BusData.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BusData.dll"]