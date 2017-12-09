using SQLite;

namespace Ams.Forms.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
