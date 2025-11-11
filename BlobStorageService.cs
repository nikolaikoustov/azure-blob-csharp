using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

/// <summary>
/// Provides methods to interact with Azure Blob Storage.
/// </summary>
public class BlobStorageService
{
    private readonly BlobServiceClient blobServiceClient;

    /// <summary>
    /// Initializes a new instance of BlobStorageService using a connection string.
    /// </summary>
    /// <param name="connectionString">Azure Blob Storage connection string.</param>
    public BlobStorageService(string connectionString)
    {
        blobServiceClient = new BlobServiceClient(connectionString);
    }

    /// <summary>
    /// Initializes a new instance of BlobStorageService using a service Uri and Azure TokenCredential.
    /// </summary>
    /// <param name="serviceUri">The Uri of the Blob service.</param>
    /// <param name="credential">The Azure TokenCredential.</param>
    public BlobStorageService(Uri serviceUri, TokenCredential credential)
    {
        blobServiceClient = new BlobServiceClient(serviceUri, credential);
    }

    /// <summary>
    /// Uploads a file to the specified blob container.
    /// </summary>
    /// <param name="containerName">The name of the blob container.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UploadFileAsync(string containerName, string filePath)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(System.IO.Path.GetFileName(filePath));
        using var fileStream = System.IO.File.OpenRead(filePath);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        Console.WriteLine($"Uploaded file '{filePath}' to container '{containerName}'.");
        return;
    }

    /// <summary>
    /// Checks if the specified blob container exists.
    /// </summary>
    /// <param name="containerName">The name of the blob container.</param>
    /// <returns>True if the container exists, otherwise false.</returns>
    public async Task<bool> ContainerExistsAsync(string containerName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var response = await containerClient.ExistsAsync();

        return response.Value;
    }

    /// <summary>
    /// Creates the specified blob container if it does not exist.
    /// </summary>
    /// <param name="containerName">The name of the blob container.</param>
    public async Task CreateContainerIfNotExistsAsync(string containerName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync();

        return;
    }

    /// <summary>
    /// Lists all blobs in the specified container. Creates the container if it does not exist.
    /// </summary>
    /// <param name="containerName">The name of the blob container.</param>
    public async Task ListBlobsAsync(string containerName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            Console.WriteLine(blobItem.Name);
        }
    }
}
