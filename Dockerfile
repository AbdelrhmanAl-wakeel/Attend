# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["AttendBackend/AttendBackend.csproj", "AttendBackend/"]
RUN dotnet restore "AttendBackend/AttendBackend.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/AttendBackend"
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image for final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port (match with Program.cs or launch settings)
EXPOSE 5095

ENTRYPOINT ["dotnet", "AttendBackend.dll"]