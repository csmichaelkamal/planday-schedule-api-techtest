namespace Planday.Schedule
{
    public class Employee
    {
        public Employee(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }    
}

