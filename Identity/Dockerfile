FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ../../Services/Identity/Identity.Service.csproj ../../Services/Identity/
RUN dotnet restore ../../Services/Identity/Identity.Service.csproj
COPY . .
WORKDIR /src/../../Services/Identity
RUN dotnet build Identity.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Identity.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Service.dll"]
