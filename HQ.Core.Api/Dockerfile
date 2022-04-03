#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["HQCore Api/HQ.SessionAPI.csproj", "HQCore Api/"]
COPY ["HQCoreLib/HQ.Core.csproj", "HQCoreLib/"]
RUN dotnet restore "HQCore Api/HQ.SessionAPI.csproj"
COPY . .
WORKDIR "/src/HQCore Api"
RUN dotnet build "HQ.SessionAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HQ.SessionAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HQ.SessionAPI.dll"]