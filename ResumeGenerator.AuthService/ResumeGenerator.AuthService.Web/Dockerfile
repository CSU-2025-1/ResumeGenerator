FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResumeGenerator.AuthService.Web/ResumeGenerator.AuthService.Web.csproj", "ResumeGenerator.AuthService.Web/"]
COPY ["ResumeGenerator.AuthService.Application/ResumeGenerator.AuthService.Application.csproj", "ResumeGenerator.AuthService.Application/"]
COPY ["ResumeGenerator.AuthService.Data/ResumeGenerator.AuthService.Data.csproj", "ResumeGenerator.AuthService.Data/"]
RUN dotnet restore "ResumeGenerator.AuthService.Web/ResumeGenerator.AuthService.Web.csproj"
COPY . .
WORKDIR "/src/ResumeGenerator.AuthService.Web"
RUN dotnet build "ResumeGenerator.AuthService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ResumeGenerator.AuthService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResumeGenerator.AuthService.Web.dll"]
