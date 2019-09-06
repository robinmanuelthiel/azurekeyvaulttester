#######################################################
# Step 1: Build the application in a container        #
#######################################################
# Download the official ASP.NET Core SDK image
# to build the project while creating the docker image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build
WORKDIR /app

# Restore NuGet packages
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the files over
COPY . .

# Build the application
RUN dotnet publish --output /out/ --configuration Debug

#######################################################
# Step 2: Run the build outcome in a container        #
#######################################################
# Download the official ASP.NET Core Runtime image
# to run the compiled application
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app

# Open port
EXPOSE 8080

# Copy the build output from the SDK image
COPY --from=build /out .

# Start the application
ENTRYPOINT ["dotnet", "KeyVaultTester.dll"]
