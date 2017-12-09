using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureMediaService.Entities;
using AzureMediaService.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMediaService.Controllers
{
    public class EncodeMediaService
    {
        //App.config file values
        private readonly string _aAdTenantDomain = CloudConfigurationManager.GetSetting("AADTenantDomain");
        private readonly string _mediaServiceRestapiEndpoint = CloudConfigurationManager.GetSetting("MediaServiceRESTAPIEndpoint");
        private readonly string _mediaServiceStorage = CloudConfigurationManager.GetSetting("MediaServicesStorage");

        //service context
        private static CloudMediaContext _context;

        private const string StandardEncoder = "Media Encoder Standard";
        private const string Bitrate = "H264 Multiple Bitrate 720p";
        private const string AdaptiveBitrate = "Adaptive Bitrate MP4";

        public void Execute(MediaContentModel mediaContent)
        {
            var tokenCredentials =
                new AzureAdTokenCredentials(_aAdTenantDomain, AzureEnvironments.AzureCloudEnvironment);
            var tokenProvider = new AzureAdTokenProvider(tokenCredentials);

            _context = new CloudMediaContext(new Uri(_mediaServiceRestapiEndpoint), tokenProvider);

            var asset = UploadFile(mediaContent.MediaFilePath, AssetCreationOptions.None);

            var encodedAsset = EncodeToAdaptiveBitrateMp4(asset, AssetCreationOptions.None);

            PublishAssetGetUrl(encodedAsset, mediaContent);
        }

        public IAsset UploadFile(string fileName, AssetCreationOptions options)
        {
            //CreateFromFile creates a new asset into which the specified source file is uploaded.
            var inputAsset = _context.Assets.CreateFromFile(fileName, options,
                (af, p) =>
                    Debug.WriteLine("Uploading '{0}' - progress: {1:0.##}%", af.Name, p.Progress));

            Debug.WriteLine($"Asset {inputAsset.Id} created");

            return inputAsset;
        }

        /// Prepare a job with a single task to transcode the specified asset
        /// into a multi-bitrate asset.
        public IAsset EncodeToAdaptiveBitrateMp4(IAsset asset, AssetCreationOptions options)
        {
            var job = _context.Jobs.CreateWithSingleTask(StandardEncoder, 
                Bitrate,
                asset,
                AdaptiveBitrate,
                options);

            Debug.WriteLine("Submitting transcoding job...");

            //submit job, wait for completion
            job.Submit();

            job = job.StartExecutionProgressTask(
                j =>
                {
                    Debug.WriteLine($"Job state: {j.State}");
                    Debug.WriteLine("Job Progress: {0:0.##}%", j.GetOverallProgress());
                },
                CancellationToken.None).Result;

            Debug.WriteLine("Transcoding job finished");

            var outputAsset = job.OutputMediaAssets[0];

            return outputAsset;
        }

        /// Publish the output asset by creating an Origin locator for adaptive streaming,
        /// and a SAS locator for progressive download.
        public void PublishAssetGetUrl(IAsset asset, MediaContentModel mediaContent)
        {
            _context.Locators.Create(
                LocatorType.OnDemandOrigin,
                asset,
                AccessPermissions.Read,
                TimeSpan.FromDays(30));

            _context.Locators.Create(
                LocatorType.Sas,
                asset,
                AccessPermissions.Read,
                TimeSpan.FromDays(30)
            );

            // Get the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming,
            // and the Progressive Download URL.
            var assetContent = new MediaContentEntity
            {
                PartitionKey = mediaContent.MediaName.Trim().Replace(" ", "-"),
                RowKey = (DateTimeOffset.MaxValue.Ticks - DateTimeOffset.UtcNow.Ticks).ToString("d19"),
                UriSmoothStreaming = asset.GetSmoothStreamingUri().ToString(),
                UriHls = asset.GetHlsUri().ToString(),
                UriMpegDash = asset.GetMpegDashUri().ToString()
            };

            Debug.WriteLine("Urls for Adaptive streaming:");
            Debug.WriteLine(assetContent.UriSmoothStreaming);
            Debug.WriteLine(assetContent.UriHls);
            Debug.WriteLine(assetContent.UriMpegDash);

            StoreUriInTable(assetContent);
        }

        public void StoreUriInTable(MediaContentEntity assetContent)
        {
            var storageAccount = CloudStorageAccount.Parse(_mediaServiceStorage);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("mediaassets");

            table.CreateIfNotExists();

            var tableOperation = TableOperation.InsertOrMerge(assetContent);

            table.Execute(tableOperation);
        }
    }
}