using AzureMediaService.Entities;
using AzureMediaService.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureMediaService.Services
{
    public class MediaListService
    {
        private readonly string _mediaServiceStorage = CloudConfigurationManager.GetSetting("MediaServicesStorage");
        public MediaContentModel Retreive()
        {
            var storageAccount = CloudStorageAccount.Parse(_mediaServiceStorage);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("mediaassets");

            table.CreateIfNotExists();

            var tableOperation = new TableQuery<MediaContentEntity>();
            var mediaList = table.ExecuteQuery(tableOperation).OrderBy(x=>x.PartitionKey);

            var content = new MediaContentModel
            {                
                ContentList = mediaList.ToList()
            };

            return content;
        }
    }
}