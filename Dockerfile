FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

COPY ["src/Fanex.DepositLicenseeAPI/Fanex.DepositLicenseeAPI.csproj", "./Fanex.DepositLicenseeAPI/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositLicenseeAPI/Fanex.DepositLicenseeAPI.csproj" \ 
    -v n -r linux-x64

COPY src .

WORKDIR /src/Fanex.DepositLicenseeAPI/

RUN dotnet publish --use-current-runtime --self-contained false -o /app  

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h2 AS base

# Don’t run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

ENV COMPlus_EnableDiagnostics=0
WORKDIR /app
COPY --from=builder /app .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.DepositLicenseeAPI.dll"]
