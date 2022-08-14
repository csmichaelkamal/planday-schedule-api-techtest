using Planday.Schedule.Infrastructure.Providers.Interfaces;

namespace Planday.Schedule.Infrastructure.Queries
{
    public class ShiftQueryBase
    {
        protected readonly IConnectionStringProvider _connectionStringProvider;

        public ShiftQueryBase(IConnectionStringProvider connectionStringProvider)
        {
            connectionStringProvider = _connectionStringProvider;
        }

        protected record ShiftDto(long Id, long? EmployeeId, string Start, string End);
    }    
}

