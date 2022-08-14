namespace Planday.Schedule.Queries
{
    public interface IGetAllShiftsQuery
    {
        Task<IReadOnlyCollection<Shift>> QueryAsync();
    }    
}

