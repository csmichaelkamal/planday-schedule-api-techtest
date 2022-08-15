using Dapper;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetEmployeeByIdQuery : IGetEmployeeByIdQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider _connectionStringProvider;

        #endregion

        #region Ctor

        public GetEmployeeByIdQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        #endregion

        #region Public Methods

        public async Task<Employee?> QueryAsync(long id)
        {
            using var sqlConnection = new SQLiteConnection(_connectionStringProvider.GetConnectiongString());

            var queryParams = new { Id = id };

            var employeeDto = await sqlConnection.QueryFirstOrDefaultAsync<EmployeeDto>(GetEmployeeByIdSql, queryParams);

            if (employeeDto is null)
            {
                // Logger
                // Cannot Find Employee with Id = id
                return null;
            }

            return new Employee(employeeDto.Id, employeeDto.Name);
        }

        #endregion

        #region DTOs

        private record EmployeeDto (long Id, string Name);

        #endregion

        #region SQL Queries 

        private const string GetEmployeeByIdSql = @"SELECT Id, Name FROM Employee WHERE Id = @Id;";

        #endregion
    }
}
