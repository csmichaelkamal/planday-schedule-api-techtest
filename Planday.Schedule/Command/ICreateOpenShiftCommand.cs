namespace Planday.Schedule.Commands
{
    public interface ICreateOpenShiftCommand
    {
        Task<long> CreateOpenShiftAsync(DateTime shiftStartDate, DateTime shiftEndDate);
    }    
}

