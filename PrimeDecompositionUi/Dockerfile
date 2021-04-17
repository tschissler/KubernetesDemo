FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY Dtos/ /src/Dtos/
COPY PrimeDecompositionUi/ /src/PrimeDecompositionUi/
WORKDIR /src/PrimeDecompositionUi
RUN dotnet restore
RUN dotnet publish "PrimeDecompositionUi.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80
ENTRYPOINT [ "dotnet", "PrimeDecompositionUi.dll" ]

