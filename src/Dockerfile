#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WorkforceManagement.WEB/WorkforceManagement.WEB.csproj", "WorkforceManagement.WEB/"]
COPY ["WorkforeManagement.BLL/WorkforceManagement.BLL.csproj", "WorkforeManagement.BLL/"]
COPY ["WorkforceManagment.Models/WorkforceManagement.Models.csproj", "WorkforceManagment.Models/"]
COPY ["WorkforceManagement.DAL/WorkforceManagement.DAL.csproj", "WorkforceManagement.DAL/"]
RUN dotnet restore "WorkforceManagement.WEB/WorkforceManagement.WEB.csproj"
COPY . .
WORKDIR "/src/WorkforceManagement.WEB"
RUN dotnet build "WorkforceManagement.WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkforceManagement.WEB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkforceManagement.WEB.dll"]