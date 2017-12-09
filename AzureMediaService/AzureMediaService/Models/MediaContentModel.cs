using AzureMediaService.Entities;
using System.Collections.Generic;

namespace AzureMediaService.Models
{
    public class MediaContentModel
    {
        public string MediaName { get; set; }

        public string MediaFileLocalPath { get; set; }

        public List<MediaContentEntity> ContentList { get; set; }
    }
}