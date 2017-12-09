using AzureMediaService.Entities;
using System.Collections.Generic;

namespace AzureMediaService.Models
{
    public class MediaContentModel
    {
        public string MediaName { get; set; }

        public string MediaFilePath { get; set; }

        public List<MediaContentEntity> ContentList { get; set; }
    }
}