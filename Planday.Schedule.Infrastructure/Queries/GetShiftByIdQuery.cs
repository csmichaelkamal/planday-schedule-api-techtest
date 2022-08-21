using Dapper;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetShiftByIdQuery : ShiftQueryBase, IGetShiftByIdQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ISqliteConnectionFactory sqliteConnectionFactory;

        #endregion

        #region Ctor

        public GetShiftByIdQuery(IConnectionStringProvider connectionStringProvider,
            ISqliteConnectionFactory sqliteConnectionFactory)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        #endregion

        #region Public Methods

        public async Task<Shift?> QueryAsync(long id)
        {
            using var sqlConnection = sqliteConnectionFactory.GetSqliteConnection(connectionStringProvider.GetConnectiongString());

            var idParam = new { Id = id };

            var shiftDto = await sqlConnection.QueryFirstOrDefaultAsync<ShiftDto>(Sql, idParam);

            // We should use a Entity Mapper here, like AutoMapper

            return shiftDto is null ? null : new Shift(shiftDto.Id,
                shiftDto.EmployeeId,
                DateTime.Parse(shiftDto.Start),
                DateTime.Parse(shiftDto.End));
        }

        #endregion

        #region SQL Queries 

        private const string Sql = @"SELECT Id, EmployeeId, Start, End FROM Shift WHERE Id = @Id";

        #endregion
    }
}
