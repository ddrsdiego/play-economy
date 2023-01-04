FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Play.Customer.Service/Play.Customer.Service.csproj", "src/Play.Customer.Service/"]
COPY ["src/Play.Customer.Core/Play.Customer.Core.csproj", "src/Play.Customer.Core/"]
RUN dotnet restore "src/Play.Customer.Service/Play.Customer.Service.csproj"
COPY . .
WORKDIR "/src/src/Play.Customer.Service"
RUN dotnet build "Play.Customer.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Play.Customer.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Play.Customer.Service.dll"]

# workaround to https://github.com/dotnet/dotnet-docker/issues/3274
ARG Logging__Console__FormatterName=""
ENV Logging__Console__FormatterName=$Logging__Console__FormatterName

ARG BUILD_DATE
ARG BUILD_VERSION
LABEL org.label-schema.build-date=$BUILD_DATE
LABEL org.label-schema.version=$BUILD_VERSION
LABEL org.label-schema.name="Play Economy with Dapr and .NET"
LABEL org.label-schema.description=".NET 6.0 and Dapr 1.8"
LABEL org.label-schema.schema-version="1.0"
