# Azure Blob C# Sample

This project demonstrates how to interact with Azure Blob Storage using C#. It supports local development and deployment to Azure Container Instances (ACI) via GitHub Actions.

## Prerequisites
- .NET 8 SDK
- Azure subscription
- Azure Blob Storage account
- Docker (for containerization)
- GitHub repository with required secrets

## Local Build & Run

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd azure-blob-csharp
   ```

2. **Configure Azure settings:**
   Edit `appsettings.json` and set your Azure Blob Storage connection string for local runs.

   Before running the application, you must add your Azure Blob Storage connection string to `appsettings.json` in the project root.

    Example `appsettings.json` content:

    ```json
    {
        "AzureBlobStorage": {
            "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<your-account>;AccountKey=<your-key>;EndpointSuffix=core.windows.net",
            "ContainerName": "<your-container-name>"
        }
    }
    ```

3. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

4. **Build the project:**
   ```bash
   dotnet build
   ```

5. **Run the project:**
   ```bash
   dotnet run -- -containerName <your-container> -fileName data/customers.csv
   ```
   - You can omit `-fileName` to only list blobs.

## Docker Build & Run

1. **Build the Docker image:**
   ```bash
   docker build -t azure-blob-csharp . \
     --build-arg CONTAINER_NAME=<your-container>
   ```

2. **Run the container:**
   ```bash
   docker run -e CONTAINER_NAME=<your-container> azure-blob-csharp
   ```

## GitHub Actions Deployment

1. **Configure repository secrets:**
   - `AZURE_CONTAINER_REGISTRY`, `AZURE_CONTAINER_REGISTRY_USERNAME`, `AZURE_CONTAINER_REGISTRY_PASSWORD`
   - `AZURE_RESOURCE_GROUP`, `AZURE_CONTAINER_INSTANCE`, `AZURE_REGION`, `AZURE_CREDENTIALS`
   - `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`

2. **Manual deployment:**
   - Go to GitHub Actions in your repo.
   - Select the `Deploy to Azure Container Instances` workflow.
   - Click "Run workflow" and set the `container_name` input.

3. **Pipeline will:**
   - Build and push the Docker image
   - Deploy to Azure Container Instances
   - Pass the container name and identity variables to the running container

## Data Upload Example

A sample CSV file is provided in `data/customers.csv`. You can upload it using the `-fileName` parameter.

---

For troubleshooting or more details, see the source code and comments in each file.

