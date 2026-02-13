# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY EmailVerifyService/EmailVerifyService.csproj EmailVerifyService/
RUN dotnet restore EmailVerifyService/EmailVerifyService.csproj

# Copiar todo el código
COPY . .
WORKDIR /src/EmailVerifyService
RUN dotnet build EmailVerifyService.csproj -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish
RUN dotnet publish EmailVerifyService.csproj -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto 8448
EXPOSE 8448

# Configurar la aplicación para escuchar en el puerto 8448
ENV ASPNETCORE_URLS=http://+:8448

ENTRYPOINT ["dotnet", "EmailVerifyService.dll"]
