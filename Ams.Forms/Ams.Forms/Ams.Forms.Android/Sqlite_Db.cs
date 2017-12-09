
using System.IO;
using Ams.Forms.Droid;
using Ams.Forms.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sqlite_Db))]
namespace Ams.Forms.Droid
{
    public class Sqlite_Db : ISQLite
    {
        public Sqlite_Db()
        {
        }

        public SQLiteConnection GetConnection()
        {
            var fileName = "MediaContent.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            var connection = new SQLiteConnection(path);

            return connection;
        }
    }
}