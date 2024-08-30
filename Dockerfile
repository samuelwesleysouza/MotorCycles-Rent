# Use a imagem base do SDK do .NET para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copie os arquivos de projeto e restaure as dependências
COPY MotorCycles-RentApi/MotorCyclesRentApi.csproj ./MotorCycles-RentApi/
COPY Aplicattion/MotorCyclesRentAplicattion.csproj ./Aplicattion/
COPY Domain/MotorCyclesRentDomain.csproj ./Domain/
COPY Infrastructure/MotorCyclesRentInfrastructure.csproj ./Infrastructure/
RUN dotnet restore MotorCycles-RentApi/MotorCyclesRentApi.csproj

# Copie o restante do código e compile a aplicação
COPY . ./
RUN dotnet publish MotorCycles-RentApi/MotorCyclesRentApi.csproj -c Release -o /app/publish

# Use uma imagem base do ASP.NET para criar a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotorCyclesRentApi.dll"]
