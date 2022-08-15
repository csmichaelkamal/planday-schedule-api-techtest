using Dapper;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetShiftByIdQuery : IGetShiftByIdQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider _connectionStringProvider;

        #endregion

        #region Ctor

        public GetShiftByIdQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        #endregion

        #region Public Methods

        public async Task<Shift?> QueryAsync(long id)
        {
            await using var sqlConnection = new SQLiteConnection(_connectionStringProvider.GetConnectiongString());

            var idParam = new { Id = id };

            var shiftDto = await sqlConnection.QueryFirstOrDefaultAsync<ShiftDto>(Sql, idParam);

            if (shiftDto is null)
            {
                return null;
            }

            // We should use a Entity Mapper here, like AutoMapper
            var shift = new Shift(shiftDto.Id,
                                  shiftDto.EmployeeId,
                                  DateTime.Parse(shiftDto.Start),
                                  DateTime.Parse(shiftDto.End));

            return shift;
        }

        #endregion

        #region Private

        private record ShiftDto(long Id, long? EmployeeId, string Start, string End);

        private const string Sql = @"SELECT Id, EmployeeId, Start, End FROM Shift WHERE Id = @Id";

        #endregion
    }
}
