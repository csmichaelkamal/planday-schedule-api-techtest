namespace Planday.Schedule.Infrastructure.Tests.SQLStatements
{
    public static class Queries
    {
        public static string GetShiftByIdSQLQuery = "SELECT Id, EmployeeId, Start, End FROM Shift WHERE Id = {0}";
    }
}
