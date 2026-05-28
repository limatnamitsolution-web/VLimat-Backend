FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

WORKDIR /src/VLimat.Eduz.App/VLimat.Eduz.App

RUN dotnet restore VLimat.Eduz.App.csproj

RUN dotnet publish VLimat.Eduz.App.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "VLimat.Eduz.App.dll"]
