FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY Dtos/ /src/Dtos/
COPY NumberGenerator/ /src/NumberGenerator/
WORKDIR /src/NumberGenerator
RUN dotnet restore
RUN dotnet publish "NumberGenerator.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80
ENTRYPOINT [ "dotnet", "NumberGenerator.dll" ]

