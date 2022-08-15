using Dapper;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetEmployeeShiftQuery : IGetEmployeeShiftQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider _connectionStringProvider;
        private long id;

        #endregion

        #region Ctor

        public GetEmployeeShiftQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        #endregion

        #region Public Methods

        public async Task<Shift?> QueryAsync(long employeeId, string startDateTime)
        {
            using var sqlConnection = new SQLiteConnection(_connectionStringProvider.GetConnectiongString());

            var queryParams = new { EmployeeId = employeeId, Start = startDateTime[..11] };

            var shiftDto = await sqlConnection.QueryFirstOrDefaultAsync<ShiftDto>(GetEmployeeShiftSql, queryParams);

            if (shiftDto is null)
            {
                // Logger
                // Cannot Find A Shift with EmployeeId = employeeId and Start = StartDateTime
                return null;
            }

            return new Shift(id = shiftDto.Id,
                             employeeId = shiftDto.EmployeeId ?? default,
                             DateTime.Parse(shiftDto.Start),
                             DateTime.Parse(shiftDto.End));
        }

        #endregion

        #region DTOs

        private record ShiftDto(long Id, long? EmployeeId, string Start, string End);

        #endregion

        #region SQL Queries 

        // Assuming that each Shift is in the one day (The same day)
        // This Query is for getting all the Shifts for certain Employee on the same day that is the new shift should be assigned
        // In read world application, we should use StringBuilder if we want to construct a string with multiple substrings
        private const string GetEmployeeShiftSql = "SELECT Id, EmployeeId, Start, End FROM Shift WHERE EmployeeId = @EmployeeId AND SUBSTR(Start, 0, 11) = @Start;";

        #endregion
    }
}
