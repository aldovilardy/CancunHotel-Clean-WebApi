#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CancunHotel.WebApi/CancunHotel.WebApi.csproj", "src/CancunHotel.WebApi/"]
COPY ["src/CancunHotel.Infrastructure/CancunHotel.Infrastructure.csproj", "src/CancunHotel.Infrastructure/"]
COPY ["src/CancunHotel.Domain/CancunHotel.Domain.csproj", "src/CancunHotel.Domain/"]
COPY ["src/CancunHotel.Application/CancunHotel.Application.csproj", "src/CancunHotel.Application/"]
RUN dotnet restore "src/CancunHotel.WebApi/CancunHotel.WebApi.csproj"
COPY . .
WORKDIR "/src/src/CancunHotel.WebApi"
RUN dotnet build "CancunHotel.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CancunHotel.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CancunHotel.WebApi.dll"]