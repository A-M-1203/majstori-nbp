FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["majstori-nbp-server.csproj", "."]
RUN dotnet restore "majstori-nbp-server.csproj"

COPY . .
RUN dotnet build "majstori-nbp-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "majstori-nbp-server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5104
COPY ./images ./images
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "majstori-nbp-server.dll"]
