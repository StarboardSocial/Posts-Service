FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9002

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StarboardSocial.PostsService.Server/StarboardSocial.PostsService.Server.csproj", "StarboardSocial.PostsService.Server/"]
RUN dotnet restore "StarboardSocial.PostsService.Server/StarboardSocial.PostsService.Server.csproj"
COPY . .
WORKDIR "/src/StarboardSocial.PostsService.Server"
RUN dotnet build "StarboardSocial.PostsService.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StarboardSocial.PostsService.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:9002
ENTRYPOINT ["dotnet", "StarboardSocial.PostsService.Server.dll"]
