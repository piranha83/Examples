#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /App
EXPOSE 80
EXPOSE 443

ENV INDOCKER=1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["App/App.csproj", "App/"]

#RUN dotnet tool install --global dotnet-ef (add package Microsoft.EntityFrameworkCore.Design)

# NodeJS 15
RUN curl -sL https://deb.nodesource.com/setup_15.x | bash - && \
    apt-get install -y build-essential nodejs

RUN npm install @angular/cli -g

RUN dotnet restore "App/App.csproj"
COPY . .
WORKDIR "/src/App"
RUN dotnet build "App.csproj" -c Release -o /App/build

FROM build AS publish
RUN dotnet publish "App.csproj" -c Release -o /App/publish

FROM base AS final
WORKDIR /App
COPY --from=publish /App .
WORKDIR /app/ClientApp
RUN npm install
RUN npm start
ENTRYPOINT ["dotnet", "App.dll"]