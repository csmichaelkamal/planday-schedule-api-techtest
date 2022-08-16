using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Factories
{
    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        #region Private Members

        protected readonly IConnectionStringProvider connectionStringProvider;

        #endregion

        #region Ctor

        public SqliteConnectionFactory(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        #endregion

        public SQLiteConnection GetSqliteConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            try
            {
                return new SQLiteConnection(connectionString);
            }
            catch
            {
                // Log Error while creation SqliteConnection
                throw;
            }
        }
    }
}
