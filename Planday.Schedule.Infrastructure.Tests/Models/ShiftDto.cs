namespace Planday.Schedule.Infrastructure.Tests.Models
{
    public record ShiftDto(long Id, long? EmployeeId, string Start, string End);
}
