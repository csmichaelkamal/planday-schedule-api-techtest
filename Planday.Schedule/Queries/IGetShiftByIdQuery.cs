namespace Planday.Schedule.Queries
{
    public interface IGetShiftByIdQuery
    {
        Task<Shift?> QueryAsync(long id);
    }    
}

