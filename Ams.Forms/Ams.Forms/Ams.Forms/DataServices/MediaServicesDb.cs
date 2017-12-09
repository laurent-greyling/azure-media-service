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

            var newItems = new List<MediaContentModel>();
            foreach (var item in content)
            {
                foreach (var mediaContent in currentContents)
                {
                    if (mediaContent.MediaName == item.MediaName)
                        continue;

                    newItems.Add(item);
                }

                _connection.InsertAll(newItems);
            }
        }

        public void RemoveContent(string name)
        {
            var itm = GetMediaContent().FirstOrDefault(x => x.MediaName == name);

            _connection.Delete(itm);
        }
    }
}
