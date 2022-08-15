namespace Planday.Schedule.Commands
{
    public interface IAssignShiftToEmployeeCommand
    {
        Task<bool> AssignShiftToEmployeeAsync(long shiftId, long employeeId);
    }    
}

