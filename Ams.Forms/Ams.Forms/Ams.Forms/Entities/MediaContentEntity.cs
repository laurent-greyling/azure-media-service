using Microsoft.WindowsAzure.Storage.Table;

namespace Ams.Forms.Entities
{
    public class MediaContentEntity : TableEntity
    {
        public string UriSmoothStreaming { get; set; }

        public string UriHls { get; set; }

        public string UriMpegDash { get; set; }
    }
}
