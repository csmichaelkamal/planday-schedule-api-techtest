namespace Planday.Schedule.Queries
{
    public interface IGetEmployeeShiftQuery
    {
        Task<Shift?> QueryAsync(long employeeId, string startDateTime);
    }    
}

