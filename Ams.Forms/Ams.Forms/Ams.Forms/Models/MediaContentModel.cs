using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ams.Forms.Models
{
    public class MediaContentModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
        public string MediaName { get; set; }

        public string MediaUri { get; set; }

        public MediaContentModel()
        {
        }
    }
}
