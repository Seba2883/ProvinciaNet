FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "src/ApiInventory.WebApi/ApiInventory.WebApi.csproj"

RUN dotnet publish "src/ApiInventory.WebApi/ApiInventory.WebApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiInventory.WebApi.dll"]