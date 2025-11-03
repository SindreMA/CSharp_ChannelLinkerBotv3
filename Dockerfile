# Use the official .NET Core SDK image for building
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY CSharp_ChannelLinkerBotv3/CSharp_ChannelLinkerBotv3.csproj CSharp_ChannelLinkerBotv3/
RUN dotnet restore "CSharp_ChannelLinkerBotv3/CSharp_ChannelLinkerBotv3.csproj"

# Copy the rest of the application code
COPY CSharp_ChannelLinkerBotv3/ CSharp_ChannelLinkerBotv3/

# Build the application
WORKDIR /src/CSharp_ChannelLinkerBotv3
RUN dotnet build "CSharp_ChannelLinkerBotv3.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CSharp_ChannelLinkerBotv3.csproj" -c Release -o /app/publish

# Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create config directory
RUN mkdir -p /app/config

# Set environment variable to use the config directory
ENV CONFIG_DIR=/app/config

# Run the application
ENTRYPOINT ["dotnet", "CSharp_ChannelLinkerBotv3.dll"]
