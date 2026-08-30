FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/HelpDesk.Web/HelpDesk.Web.csproj", "src/HelpDesk.Web/"]
RUN dotnet restore "src/HelpDesk.Web/HelpDesk.Web.csproj"

COPY . .
WORKDIR "/src/src/HelpDesk.Web"
RUN dotnet publish "HelpDesk.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "HelpDesk.Web.dll"]
