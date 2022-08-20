using Newtonsoft.Json;

namespace Planday.Schedule
{
    public class ShiftEmployee : Shift
    {
        /// <summary>
        /// Employee Name
        /// </summary>
        [JsonProperty("name")]
        public string EmployeeName { get; }

        /// <summary>
        /// Employee Email 
        /// </summary>
        [JsonProperty("email")]
        public string EmployeeEmail { get; }

        public ShiftEmployee(long shiftId, long employeeId, DateTime start, DateTime end, string employeeName, string employeeEmail) :
            base(shiftId, employeeId, start, end)
        {
            EmployeeName = employeeName;
            EmployeeEmail = employeeEmail;

        }
    }
}

