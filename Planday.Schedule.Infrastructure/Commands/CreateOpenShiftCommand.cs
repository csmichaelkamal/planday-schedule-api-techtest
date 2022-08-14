using Dapper;
using Planday.Schedule.Commands;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Commands
{
    public class CreateOpenShiftCommand : ICreateOpenShiftCommand
    {
        #region Private Members

        private readonly IConnectionStringProvider _connectionStringProvider;

        private record ShiftDto(long Id, DateTime StartDate, DateTime EndDate);

        #endregion

        #region Ctor

        public CreateOpenShiftCommand(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        #endregion

        #region Public Methods

        public async Task<long> CreateOpenShiftAsync(DateTime startDate, DateTime endDate)
        {
            var areShiftsDatesValid = ValidateShiftDates(startDate, endDate);

            if (!areShiftsDatesValid)
            {
                // Logger
                // endDate must be greater than the startDate
                return 0;
            }

            await using var sqlConnection = new SQLiteConnection(_connectionStringProvider.GetConnectiongString());

            await sqlConnection.OpenAsync();

            var insertionCmd = new SQLiteCommand(OpenShiftInsertionSql, sqlConnection);

            var commandParams = new
            {
                Start = startDate.ToString("u"),
                End = endDate.ToString("u")
            };

            insertionCmd.Parameters.AddWithValue(nameof(commandParams.Start), commandParams.Start);
            insertionCmd.Parameters.AddWithValue(nameof(commandParams.End), commandParams.End);

            var rowsInserted = await insertionCmd.ExecuteNonQueryAsync();
            
            long lastInsertedId;

            if (rowsInserted > 0)
            {
                lastInsertedId = sqlConnection.LastInsertRowId;
                await sqlConnection.CloseAsync();
                return lastInsertedId;
            }

            await sqlConnection.CloseAsync();
            return rowsInserted;
        }

        #endregion

        #region Helpers

        private bool ValidateShiftDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Day != endDate.Day)
            {
                // Logger
                // Start Date and End Date Must be on the same day
                return false;
            }

            if ((endDate - startDate).TotalMinutes < 0)
            {
                // Logger
                // EndDate Must be Greater that the StartDate
                return false;
            }

            return true;
        }

        #endregion

        #region Queries

        private const string OpenShiftInsertionSql = "INSERT INTO Shift (Start, End) VALUES (@Start, @End)";

        #endregion
    }
}