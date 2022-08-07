#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EducationalPortal.API/EducationalPortal.API.csproj", "EducationalPortal.API/"]
COPY ["EducationalPortal.Infrastructure/EducationalPortal.Infrastructure.csproj", "EducationalPortal.Infrastructure/"]
COPY ["EducationalPortal.Core/EducationalPortal.Core.csproj", "EducationalPortal.Core/"]
COPY ["EducationalPortal.Application/EducationalPortal.Application.csproj", "EducationalPortal.Application/"]
RUN dotnet restore "EducationalPortal.API/EducationalPortal.API.csproj"
COPY . .
WORKDIR "/src/EducationalPortal.API"
RUN dotnet build "EducationalPortal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EducationalPortal.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EducationalPortal.API.dll"]