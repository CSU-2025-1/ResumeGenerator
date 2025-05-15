FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResumeGenerator.ApiService.Web/ResumeGenerator.ApiService.Web.csproj", "ResumeGenerator.ApiService.Web/"]
COPY ["ResumeGenerator.ApiService.Application/ResumeGenerator.ApiService.Application.csproj", "ResumeGenerator.ApiService.Application/"]
COPY ["../ResumeGenerator.Common/ResumeGenerator.Common.csproj", "../ResumeGenerator.Common/"]
COPY ["ResumeGenerator.ApiService.Data/ResumeGenerator.ApiService.Data.csproj", "ResumeGenerator.ApiService.Data/"]
COPY ["ResumeGenerator.ApiService.Grpc/ResumeGenerator.ApiService.Grpc.csproj", "ResumeGenerator.ApiService.Grpc/"]
COPY ["ResumeGenerator.ApiService.Jobs/ResumeGenerator.ApiService.Jobs.csproj", "ResumeGenerator.ApiService.Jobs/"]
RUN dotnet restore "ResumeGenerator.ApiService.Web/ResumeGenerator.ApiService.Web.csproj"
COPY . .
WORKDIR "/src/ResumeGenerator.ApiService.Web"
RUN dotnet build "./ResumeGenerator.ApiService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ResumeGenerator.ApiService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeGenerator.ApiService.Web.dll"]
