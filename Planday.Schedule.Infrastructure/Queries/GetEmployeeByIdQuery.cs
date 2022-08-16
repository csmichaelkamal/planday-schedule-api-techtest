using Dapper;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetEmployeeByIdQuery : IGetEmployeeByIdQuery
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ISqliteConnectionFactory sqliteConnectionFactory;

        #endregion

        #region Ctor

        public GetEmployeeByIdQuery(IConnectionStringProvider connectionStringProvider,
            ISqliteConnectionFactory sqliteConnectionFactory)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqliteConnectionFactory = sqliteConnectionFactory;
        }

        #endregion

        #region Public Methods

        public async Task<Employee?> QueryAsync(long id)
        {
            using var sqlConnection = sqliteConnectionFactory.GetSqliteConnection(connectionStringProvider.GetConnectiongString());

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

        private record EmployeeDto(long Id, string Name);

        #endregion

        #region SQL Queries 

        private const string GetEmployeeByIdSql = @"SELECT Id, Name FROM Employee WHERE Id = @Id;";

        #endregion
    }
}
