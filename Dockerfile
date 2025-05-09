# ----------------------------
# STAGE 1: build Angular 
# ----------------------------
FROM node:18-alpine AS node-build
WORKDIR /app/web

COPY InvestmentSimulator.Web/package*.json ./
RUN npm ci

COPY InvestmentSimulator.Web/ ./
RUN npm run build -- --configuration production

# ----------------------------
# 2) Executa testes - backend
# ----------------------------

FROM build AS testrunner
WORKDIR /src
RUN dotnet test InvestmentSimulator.Tests/InvestmentSimulator.Tests.csproj \
    --configuration Release \
    --no-build \
    --logger "trx;LogFileName=test-results.trx"

# ----------------------------
# STAGE 3: build & publish API
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-build
WORKDIR /src

COPY . .
RUN dotnet publish "InvestmentSimulator.Api/InvestmentSimulator.Api.csproj" \
    -c Release \
    -o /app/publish

# ----------------------------
# STAGE 4: runtime
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=dotnet-build /app/publish .

COPY --from=node-build /app/web/dist/investmentsimulatorweb/browser/. ./wwwroot

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "InvestmentSimulator.Api.dll"]
