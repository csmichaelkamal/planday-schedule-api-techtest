using Dapper;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetAllShiftsQuery : ShiftQueryBase, IGetAllShiftsQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ISqliteConnectionFactory sqliteConnectionFactory;

        #endregion

        #region Ctor

        public GetAllShiftsQuery(IConnectionStringProvider connectionStringProvider,
            ISqliteConnectionFactory sqliteConnectionFactory)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        #endregion

        #region Public Methods

        public async Task<IReadOnlyCollection<Shift>> QueryAsync()
        {
            await using var sqlConnection = sqliteConnectionFactory.GetSqliteConnection(connectionStringProvider.GetConnectiongString());

            var shiftDtos = await sqlConnection.QueryAsync<ShiftDto>(Sql);

            var shifts = shiftDtos.Select(x =>
                new Shift(x.Id, x.EmployeeId, DateTime.Parse(x.Start), DateTime.Parse(x.End)));

            return shifts.ToList();
        }

        #endregion

        #region SQL Queries

        private const string Sql = @"SELECT Id, EmployeeId, Start, End FROM Shift;";

        #endregion
    }
}

