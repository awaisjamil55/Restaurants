using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService : IBlobStorageService
{
    private readonly IOptions<BlobStorageSettings> _blobStorageSettingsOptions;
    private readonly BlobStorageSettings _blobStorageSettings;

    public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions)
    {
        _blobStorageSettingsOptions = blobStorageSettingsOptions;
        _blobStorageSettings = _blobStorageSettingsOptions.Value;
    }

    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(
            _blobStorageSettings.LogosContainerName
        );

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);

        return blobClient.Uri.ToString();
    }
}
