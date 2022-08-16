using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Factories.Interfaces
{
    public interface ISqliteConnectionFactory
    {
        SQLiteConnection GetSqliteConnection(string connectionString);
    }
}
