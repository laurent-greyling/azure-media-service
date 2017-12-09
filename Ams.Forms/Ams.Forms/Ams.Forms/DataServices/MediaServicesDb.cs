using Ams.Forms.Interfaces;
using Ams.Forms.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Ams.Forms.DataServices
{
    public class MediaServicesDb
    {
        private SQLiteConnection _connection;

        public MediaServicesDb()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<MediaContentModel>();
        }

        public List<MediaContentModel> GetMediaContent()
        {
            return (from tbl in _connection.Table<MediaContentModel>() select tbl).ToList();
        }

        public void InsertContent(List<MediaContentModel> content)
        {
            var currentContents = GetMediaContent();

            if (currentContents.Count < 1)
            {
                _connection.InsertAll(content);
                return;
            }

            var azure = content.Select(x => x.MediaName).ToList();
            var forms = currentContents.Select(x => x.MediaName).ToList();
            
            var newItems = azure.Except(forms);
            
            if (newItems.Count() > 0)
            {
                var addItems = new List<MediaContentModel>();
                foreach (var item in newItems)
                {
                    var itemsToAdd = content.Where(n => n.MediaName == item).ToList();

                    foreach (var mediaItem in itemsToAdd)
                    {
                        var newMedia = new MediaContentModel
                        {
                            MediaName = mediaItem.MediaName,
                            MediaUri = mediaItem.MediaUri
                        };

                        addItems.Add(newMedia);
                    }
                }

                _connection.InsertAll(addItems);
            }                    
        }

        public void RemoveContent(string name)
        {
            var itm = GetMediaContent().FirstOrDefault(x => x.MediaName == name);

            _connection.Delete(itm);
        }
    }
}
