#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CqrsApi.Core/CqrsApi.Core.csproj", "CqrsApi.Core/"]
COPY ["CqrsApi.Requests/CqrsApi.Requests.csproj", "CqrsApi.Requests/"]
COPY ["CqrsApi.Data/CqrsApi.Data.csproj", "CqrsApi.Data/"]
COPY ["CqrsApi.Models/CqrsApi.Models.csproj", "CqrsApi.Models/"]
RUN dotnet restore "CqrsApi.Core/CqrsApi.Core.csproj"
COPY . .
WORKDIR "/src/CqrsApi.Core"
RUN dotnet build "CqrsApi.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CqrsApi.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CqrsApi.Core.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet CqrsApi.Core.dll