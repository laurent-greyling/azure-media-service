using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMediaService.Entities
{
    public class MediaContentEntity : TableEntity
    {
        public string UriSmoothStreaming { get; set; }

        public string UriHls { get; set; }

        public string UriMpegDash { get; set; }
    }
}