#Etap 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#Etap 2 - kopiujemy csproj
COPY *.csproj ./
RUN dotnet restore

#Kopiujemy resztę plików
COPY . ./
RUN dotnet publish -c Release -o /app/publish

#Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build  app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT [ "dotnet", "Api.dll" ]
