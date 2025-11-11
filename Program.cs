using Azure.Identity;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Storage.Blobs; // Assuming this is needed for BlobStorageService

// See https://aka.ms/new-console-template for more information
// ...existing code...
public partial class Program
{
	/// <summary>
	/// Application entry point. Lists blobs in the specified Azure Blob container.
	/// </summary>
	static async Task Main(string[] args)
	{
		// Load configuration from appsettings.json
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		var connectionString = configuration["AzureBlob:ConnectionString"];
		var accountName = configuration["AzureBlob:AccountName"];

		// Parse named parameters -containerName <container name> and -fileName <file name>
		string? containerName = null;
		string? fileName = null;
		for (int i = 0; i < args.Length - 1; i++)
		{
			if (args[i] == "-containerName")
			{
				containerName = args[i + 1];
			}
			if (args[i] == "-fileName")
			{
				fileName = args[i + 1];
			}
		}

		if (string.IsNullOrWhiteSpace(containerName))
		{
			containerName = configuration["AzureBlob:ContainerName"];
		}

		if (string.IsNullOrWhiteSpace(containerName))
		{
			Console.WriteLine("Container name must be provided as a named parameter (-containerName <name>) or in appsettings.json.");
			return;
		}

		BlobStorageService blobService;
		if (!string.IsNullOrWhiteSpace(accountName))
		{
			// Use Uri and DefaultAzureCredential
			var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");
			var credential = new DefaultAzureCredential();
			blobService = new BlobStorageService(serviceUri, credential);
		}
		else
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				Console.WriteLine("AzureBlob:ConnectionString is missing in appsettings.json.");
				return;
			}
			blobService = new BlobStorageService(connectionString);
		}

		// Check if container exists, create if not
		if (!await blobService.ContainerExistsAsync(containerName))
		{
			Console.WriteLine($"Container '{containerName}' does not exist. Creating...");
			await blobService.CreateContainerIfNotExistsAsync(containerName);
			Console.WriteLine($"Container '{containerName}' created.");
		}

		if (!string.IsNullOrWhiteSpace(fileName))
		{
			await blobService.UploadFileAsync(containerName, fileName);
		}

		Console.WriteLine($"Listing objects in Container '{containerName}'...");
		await blobService.ListBlobsAsync(containerName);

		return;
	}
}
// ...existing code...
