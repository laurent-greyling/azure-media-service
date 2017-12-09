using AzureMediaService.Entities;
using AzureMediaService.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureMediaService.Controllers
{
    public class PlayMediaController : Controller
    {
        private readonly string _mediaServiceStorage = CloudConfigurationManager.GetSetting("MediaServicesStorage");

        [HttpPost]
        public ActionResult Index(string playMedia)
        {
            var uri = GetUri(playMedia);
            return View();
        }

        private Uri GetUri(string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(_mediaServiceStorage);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("mediaassets");

            var tableOperation = new TableQuery<MediaContentEntity>();
            var mediaList = table.ExecuteQuery(tableOperation).FirstOrDefault(n=>n.PartitionKey == fileName).UriSmoothStreaming;

            return new Uri(mediaList);
        }
    }
}
