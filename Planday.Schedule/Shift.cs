using System;

namespace Planday.Schedule
{
    public class Shift
    {
        public Shift(long id, long? employeeId, DateTime start, DateTime end)
        {
            Id = id;
            EmployeeId = employeeId;
            Start = start;
            End = end;
        }

        public long Id { get; }
        public long? EmployeeId { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
    }    
}

