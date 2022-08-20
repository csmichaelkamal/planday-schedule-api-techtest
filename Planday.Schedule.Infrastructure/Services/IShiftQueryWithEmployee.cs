namespace Planday.Schedule.Infrastructure.Services
{
    public interface IShiftQueryWithEmployee
    {
        Task<ShiftEmployee> GetShiftWithEmployeeData(long shiftId);
    }
}
