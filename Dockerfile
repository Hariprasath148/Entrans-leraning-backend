# Use the official .NET image as a build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["learning-api/learning-api.csproj", "learning-api/"]
RUN dotnet restore "learning-api/learning-api.csproj"
COPY . .
WORKDIR "/src/learning-api"
RUN dotnet build "learning-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "learning-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "learning-api.dll"]