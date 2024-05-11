using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null)
            return null;

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobNameFromUrl(blobUrl)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

        var sasToken = sasBuilder
            .ToSasQueryParameters(
                new Azure.Storage.StorageSharedKeyCredential(
                    blobServiceClient.AccountName,
                    _blobStorageSettings.AccountKey
                )
            )
            .ToString();

        return $"{blobUrl}?{sasToken}";
    }

    private string GetBlobNameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}
