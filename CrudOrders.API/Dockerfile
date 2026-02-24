# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj files and restore
COPY CrudOrders.Domain/CrudOrders.Domain.csproj CrudOrders.Domain/
COPY CrudOrders.Application/CrudOrders.Application.csproj CrudOrders.Application/
COPY CrudOrders.Infrastructure/CrudOrders.Infrastructure.csproj CrudOrders.Infrastructure/
COPY CrudOrders.API/CrudOrders.API.csproj CrudOrders.API/
RUN dotnet restore CrudOrders.API/CrudOrders.API.csproj

# Copy source and publish
COPY CrudOrders.Domain/ CrudOrders.Domain/
COPY CrudOrders.Application/ CrudOrders.Application/
COPY CrudOrders.Infrastructure/ CrudOrders.Infrastructure/
COPY CrudOrders.API/ CrudOrders.API/
RUN dotnet publish CrudOrders.API/CrudOrders.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CrudOrders.API.dll"]
