FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# Set the working directory in the container
WORKDIR /app
# Copy the project file and restore dependencies
COPY *.csproj ./

RUN dotnet restore
# Copy the rest of the application code
COPY . ./
# Build the application
RUN dotnet publish -c Release -o out
# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:10.0
# Set the working directory in the container
WORKDIR /app
# Copy the built application from the build stage
COPY --from=build-env /app/out .
# Expose the port the app runs on
EXPOSE 7190
# Set the entry point for the application
ENTRYPOINT ["dotnet", "Home Security.dll"]
# Note: Ensure that the .csproj file and the application code are in the same directory as this Dockerfile
# You can build the Docker image using the command:
# docker build -t yourapplication:latest .
# And run it using:
# docker run -d -p 8080:80 yourapplication:latest
# Ensure you have Docker installed and running on your machine
# You can replace '8080' with any port you want to map to the container's port 80
# For more information on Docker and .NET, refer to the official documentation
# https://docs.microsoft.com/en-us/dotnet/core/docker/building-images
# For debugging purposes, you can add the following line to keep the container running
# CMD ["tail", "-f", "/dev/null"]
# This will keep the container alive for debugging
# purposes, but remember to remove it in production
# For production, you should ensure that your application is properly configured
# and that you have set up logging, monitoring, and other necessary configurations
# to ensure the application runs smoothly in a containerized environment
# You can also use multi-stage builds to optimize the image size
# and reduce the attack surface by only including necessary files in the final image
# For more advanced scenarios, consider using Docker Compose
# to manage multi-container applications and orchestrate services
# For more information on Docker Compose, refer to the official documentation
# https://docs.docker.com/compose/
# For security best practices, ensure that you are using the latest base images
# and that you are following the security guidelines provided by Microsoft
# for .NET applications in containers
# https://docs.microsoft.com/en-us/dotnet/core/security/docker-security
# For performance optimization, consider using the .NET 6.0 runtime image
# which is optimized for running .NET applications in production
# https://docs.microsoft.com/en-us/dotnet/core/docker/optimize-dockerfile
# For more information on .NET 6.0 features and improvements,
# refer to the official release notes
# https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6
# For community support and discussions, consider joining the .NET community
# on GitHub or other forums
