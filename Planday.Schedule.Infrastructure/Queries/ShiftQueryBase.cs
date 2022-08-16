namespace Planday.Schedule.Infrastructure.Queries
{
    public class ShiftQueryBase
    {
        //protected readonly IConnectionStringProvider connectionStringProvider;

        //public ShiftQueryBase(IConnectionStringProvider connectionStringProvider)
        //{
        //    this.connectionStringProvider = connectionStringProvider;
        //}

        protected record ShiftDto(long Id, long? EmployeeId, string Start, string End);
    }
}

