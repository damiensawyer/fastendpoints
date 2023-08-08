FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/fastendpointsDemo/fastendpointsDemo.csproj", "fastendpointsDemo/"]
RUN dotnet restore "src/fastendpointsDemo/fastendpointsDemo.csproj"
COPY . .
WORKDIR "/src/fastendpointsDemo"
RUN dotnet build "fastendpointsDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "fastendpointsDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fastendpointsDemo.dll"]
