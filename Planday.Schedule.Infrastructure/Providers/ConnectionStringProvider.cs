using Planday.Schedule.Infrastructure.Providers.Interfaces;
using System.Text.RegularExpressions;

namespace Planday.Schedule.Infrastructure.Providers
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        #region Private Members

        private readonly string _connectionString;

        #endregion

        #region Ctor

        public ConnectionStringProvider(string connectionString)
        {
            _connectionString = ProcessConnectionString(connectionString);
        }

        #endregion

        #region Public Methods

        public string GetConnectiongString()
        {
            return _connectionString;
        }

        #endregion

        #region Private Methods

        private static string ProcessConnectionString(string connectionString)
        {
            const string pattern = "(.*=)(.*)(;.*)";
            var match = Regex.Match(connectionString, pattern);
            return Regex.Replace(
                connectionString,
                pattern,
                $"$1{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, match.Groups[2].Value)}$3");
        }

        #endregion
    }
}

