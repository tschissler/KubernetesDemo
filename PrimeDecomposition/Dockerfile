FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY Dtos/ /src/Dtos/
COPY PrimeDecomposition/ /src/PrimeDecomposition/
WORKDIR /src/PrimeDecomposition
RUN dotnet restore
RUN dotnet publish "PrimeDecomposition.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80
ENTRYPOINT [ "dotnet", "PrimeDecomposition.dll" ]

