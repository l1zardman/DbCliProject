FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DbcliProject/DbcliProject.csproj", "DbcliProject/"]
COPY ["DbcliArangoLoader/DbcliArangoLoader.csproj", "DbcliArangoLoader/"]
COPY ["DbcliModels/DbcliModels.csproj", "DbcliModels/"]
COPY ["DbcliCoreUtility/DbcliCoreUtility.csproj", "DbcliCoreUtility/"]
RUN dotnet restore "DbcliProject/DbcliProject.csproj"
COPY . .
WORKDIR "/src/DbcliProject"
RUN dotnet build "DbcliProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DbcliProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["sleep", "infinity"]
