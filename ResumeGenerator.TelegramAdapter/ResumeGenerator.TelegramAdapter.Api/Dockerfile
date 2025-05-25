FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResumeGenerator.TelegramAdapter.Api/ResumeGenerator.TelegramAdapter.Api.csproj", "ResumeGenerator.TelegramAdapter.Api/"]
COPY ["ResumeGenerator.TelegramAdapter.Core/ResumeGenerator.TelegramAdapter.Core.csproj", "ResumeGenerator.TelegramAdapter.Core/"]
COPY ["ResumeGenerator.TelegramAdapter.Grpc.Clients/ResumeGenerator.TelegramAdapter.Grpc.Clients.csproj", "ResumeGenerator.TelegramAdapter.Grpc.Clients/"]
COPY ["ResumeGenerator.TelegramAdapter.Grpc.Server/ResumeGenerator.TelegramAdapter.Grpc.Server.csproj", "ResumeGenerator.TelegramAdapter.Grpc.Server/"]
COPY ["ResumeGenerator.TelegramAdapter.Infrastructure/ResumeGenerator.TelegramAdapter.Infrastructure.csproj", "ResumeGenerator.TelegramAdapter.Infrastructure/"]
RUN dotnet restore "ResumeGenerator.TelegramAdapter.Api/ResumeGenerator.TelegramAdapter.Api.csproj"
COPY . .
WORKDIR "/src/ResumeGenerator.TelegramAdapter.Api"
RUN dotnet build "./ResumeGenerator.TelegramAdapter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ResumeGenerator.TelegramAdapter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeGenerator.TelegramAdapter.Api.dll"]
