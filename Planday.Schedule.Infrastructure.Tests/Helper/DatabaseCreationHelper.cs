using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Tests.Helper
{
    public static class DatabaseCreationHelper
    {
        private static readonly string employeeTableCreationSQLStatement = @"CREATE TABLE Employee (
                                                                               Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                               Name text NOT NULL
                                                                             );";

        private static readonly string shiftTableCreationSQLStatement = @"CREATE TABLE Shift 
                                                                        (
                                                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                            EmployeeId INTEGER,
                                                                            Start TEXT NOT NULL,
                                                                            End TEXT NOT NULL,
	                                                                        FOREIGN KEY(EmployeeId) REFERENCES Employee(Id)
                                                                        );";

        private static readonly string shiftTableInsertRowsSQLStatement = @"
                                        INSERT INTO Shift (EmployeeId, Start, End)
                                        VALUES (1, '2022-08-19 12:00:00.000', '2022-08-19 17:00:00.000');
                                        INSERT INTO Shift (EmployeeId, Start, End)
                                        VALUES (2, '2022-08-21 09:00:00.000', '2022-08-21 15:00:00.000');";

        private static readonly string employeeTableInsertRowsSQLStatement = @"
                                        INSERT INTO Employee (Name) VALUES ('John Doe');
                                        INSERT INTO Employee (Name) VALUES ('Jane Doe');";

        public static async Task CreateDatabase(SQLiteConnection sqliteConnection)
        {
            var employeeTableCreationSQLCommand = new SQLiteCommand(employeeTableCreationSQLStatement, sqliteConnection);
            var shiftTableCreationSQLCommand = new SQLiteCommand(shiftTableCreationSQLStatement, sqliteConnection);

            var employeeTableInsertionSQLCommand = new SQLiteCommand(employeeTableInsertRowsSQLStatement, sqliteConnection);
            var shiftTableInsertionSQLCommand = new SQLiteCommand(shiftTableInsertRowsSQLStatement, sqliteConnection);

            try
            {
                await sqliteConnection.OpenAsync();

                await employeeTableCreationSQLCommand.ExecuteNonQueryAsync();
                await shiftTableCreationSQLCommand.ExecuteNonQueryAsync();

                await employeeTableInsertionSQLCommand.ExecuteNonQueryAsync();
                await shiftTableInsertionSQLCommand.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                // Log Error Creating or Inserting into the database
                throw;
            }
        }
    }
}
