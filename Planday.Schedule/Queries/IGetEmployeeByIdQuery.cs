namespace Planday.Schedule.Queries
{
    public interface IGetEmployeeByIdQuery
    {
        Task<Employee?> QueryAsync(long id);
    }    
}

