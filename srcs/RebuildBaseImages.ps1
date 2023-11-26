# Build the .NET SDK base image that contains the Directory.Packages.props file so it is used when restoring the NuGet packages
docker build -t icap-dotnet-sdk-base:1.0 . -f dotnet-sdk-base-dockerfile

# Build the .NET runtime base image
docker build -t icap-dotnet-runtime-base:1.0 . -f dotnet-runtime-base-dockerfile

# Build the .NET ASP.NET base image
docker build -t icap-dotnet-aspnet-base:1.0 . -f dotnet-aspnet-base-dockerfile

# Pause to allow the user to inspect the images
pause