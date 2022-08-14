using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class GetAllShiftsQuery : IGetAllShiftsQuery
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public GetAllShiftsQuery(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
    
        public async Task<IReadOnlyCollection<Shift>> QueryAsync()
        {
            await using var sqlConnection = new SQLiteConnection(_connectionStringProvider.GetConnectiongString());

            var shiftDtos = await sqlConnection.QueryAsync<ShiftDto>(Sql);

            var shifts = shiftDtos.Select(x => 
                new Shift(x.Id, x.EmployeeId, DateTime.Parse(x.Start), DateTime.Parse(x.End)));
        
            return shifts.ToList();
        }

        private const string Sql = @"SELECT Id, EmployeeId, Start, End FROM Shift;";
    
        private record ShiftDto(long Id, long? EmployeeId, string Start, string End);
    }    
}

