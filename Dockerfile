# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BusData.csproj", "./"]
RUN dotnet restore "BusData.csproj"

# Copy remaining source code and publish
COPY . .
RUN dotnet publish "BusData.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Step 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose HTTP port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BusData.dll"]