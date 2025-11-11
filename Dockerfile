# Use official .NET image as build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "azure-blob-csharp.csproj"
RUN dotnet publish "azure-blob-csharp.csproj" -c Release -o /app/publish

# Use official .NET runtime image for final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .


# Parameterize environment variables for Azure Managed Identity
ARG AZURE_CLIENT_ID
ARG AZURE_TENANT_ID
ARG AZURE_SUBSCRIPTION_ID
ENV AZURE_CLIENT_ID=${AZURE_CLIENT_ID}
ENV AZURE_TENANT_ID=${AZURE_TENANT_ID}
ENV AZURE_SUBSCRIPTION_ID=${AZURE_SUBSCRIPTION_ID}

# Entrypoint for the app
ENTRYPOINT ["dotnet", "azure-blob-csharp.dll"]
