FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY library_manager_server/*.csproj ./library_manager_server/
COPY library_manager_server_tests/*.csproj ./library_manager_server_tests/
RUN dotnet restore

# copy everything else and build app
COPY library_manager_server/. ./library_manager_server/
WORKDIR /source/library_manager_server
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "library_manager_server.dll"]