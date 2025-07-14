FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/UI/NadinSoft.Api/NadinSoft.Api.csproj", "src/UI/NadinSoft.Api/"]
COPY ["src/Infrastructure/NadinSoft.Infrastructure.CrossCutting/NadinSoft.Infrastructure.CrossCutting.csproj", "src/Infrastructure/NadinSoft.Infrastructure.CrossCutting/"]
COPY ["src/Infrastructure/NadinSoft.Infrastructure.Identity/NadinSoft.Infrastructure.Identity.csproj", "src/Infrastructure/NadinSoft.Infrastructure.Identity/"]
COPY ["src/Infrastructure/NadinSoft.Infrastructure.Persistence/NadinSoft.Infrastructure.Persistence.csproj", "src/Infrastructure/NadinSoft.Infrastructure.Persistence/"]
COPY ["src/Core/NadinSoft.Application/NadinSoft.Application.csproj", "src/Core/NadinSoft.Application/"]
COPY ["src/Core/NadinSoft.Domain/NadinSoft.Domain/NadinSoft.Domain.csproj", "src/Core/NadinSoft.Domain/NadinSoft.Domain/"]
COPY ["src/UI/NadinSoft.WebFramework/NadinSoft.WebFramework.csproj", "src/UI/NadinSoft.WebFramework/"]
RUN dotnet restore "src/UI/NadinSoft.Api/NadinSoft.Api.csproj"
COPY . .
WORKDIR "/src/src/UI/NadinSoft.Api"
RUN dotnet build "NadinSoft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NadinSoft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NadinSoft.Api.dll"]
