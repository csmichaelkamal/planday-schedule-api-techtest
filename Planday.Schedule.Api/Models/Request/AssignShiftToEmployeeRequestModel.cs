namespace Planday.Schedule.Api.Models.Request
{
    public class AssignShiftToEmployeeRequestModel
    {
        /// <summary>
        /// Shift Id
        /// </summary>
        public long ShiftId { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public long EmployeeId { get; set; }
    }
}
