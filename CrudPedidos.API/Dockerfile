# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj files and restore
COPY CrudPedidos.Domain/CrudPedidos.Domain.csproj CrudPedidos.Domain/
COPY CrudPedidos.Application/CrudPedidos.Application.csproj CrudPedidos.Application/
COPY CrudPedidos.Infrastructure/CrudPedidos.Infrastructure.csproj CrudPedidos.Infrastructure/
COPY CrudPedidos.API/CrudPedidos.API.csproj CrudPedidos.API/
RUN dotnet restore CrudPedidos.API/CrudPedidos.API.csproj

# Copy source and publish
COPY CrudPedidos.Domain/ CrudPedidos.Domain/
COPY CrudPedidos.Application/ CrudPedidos.Application/
COPY CrudPedidos.Infrastructure/ CrudPedidos.Infrastructure/
COPY CrudPedidos.API/ CrudPedidos.API/
RUN dotnet publish CrudPedidos.API/CrudPedidos.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CrudPedidos.API.dll"]
