FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Worker/ResumeGenerator.GeneratorService.Worker.csproj", "ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Worker/"]
COPY ["ResumeGenerator.Common/ResumeGenerator.Common.csproj", "ResumeGenerator.Common/"]
COPY ["ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Core/ResumeGenerator.GeneratorService.Core.csproj", "ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Core/"]
COPY ["ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Grpc/ResumeGenerator.GeneratorService.Grpc.csproj", "ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Grpc/"]
COPY ["ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Infrastructure/ResumeGenerator.GeneratorService.Infrastructure.csproj", "ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Infrastructure/"]
RUN dotnet restore "ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Worker/ResumeGenerator.GeneratorService.Worker.csproj"
COPY . .
WORKDIR "/src/ResumeGenerator.GeneratorService/ResumeGenerator.GeneratorService.Worker"
RUN dotnet build "./ResumeGenerator.GeneratorService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ResumeGenerator.GeneratorService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeGenerator.GeneratorService.Worker.dll"]
