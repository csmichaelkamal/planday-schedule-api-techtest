using Planday.Schedule.Commands;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;
using System.Data.Common;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Commands
{
    public class AssignShiftToEmployeeCommand : IAssignShiftToEmployeeCommand
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IGetEmployeeByIdQuery getEmployeeByIdQuery;
        private readonly IGetEmployeeShiftQuery getEmployeeShiftQuery;
        private readonly IGetShiftByIdQuery getShiftByIdQuery;

        #endregion

        #region Ctor

        public AssignShiftToEmployeeCommand(IConnectionStringProvider connectionStringProvider,
            IGetEmployeeByIdQuery getEmployeeByIdQuery,
            IGetEmployeeShiftQuery getEmployeeShiftQuery,
            IGetShiftByIdQuery getShiftByIdQuery)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.getEmployeeByIdQuery = getEmployeeByIdQuery;
            this.getEmployeeShiftQuery = getEmployeeShiftQuery;
            this.getShiftByIdQuery = getShiftByIdQuery;
        }

        #endregion

        #region Public Methods

        public async Task<bool> AssignShiftToEmployeeAsync(long shiftId, long employeeId)
        {
            #region Validation

            var employee = await getEmployeeByIdQuery.QueryAsync(employeeId);

            if (employee is null)
            {
                // Logger
                // Cannot Find Employee with Id = employeeId
                return false;
            }

            var shift = await getShiftByIdQuery.QueryAsync(shiftId);

            if (shift is null)
            {
                // Logger
                // Cannot Find Shift with Id = shiftId
                return false;
            }

            var employeeShift = await getEmployeeShiftQuery.QueryAsync(employeeId, shift.Start.ToString("u"));

            if (employeeShift is not null && IsOverlappingShifts(shift, employeeShift))
            {
                return false;
            }

            #endregion

            await using var sqlConnection = new SQLiteConnection(connectionStringProvider.GetConnectiongString());

            await sqlConnection.OpenAsync();

            var insertionCmd = new SQLiteCommand(AssignShiftToEmployeeSql, sqlConnection);

            insertionCmd.Parameters.AddWithValue(nameof(shiftId), shiftId);
            insertionCmd.Parameters.AddWithValue(nameof(employeeId), employeeId);

            try
            {
                var rowsInserted = await insertionCmd.ExecuteNonQueryAsync();
                return rowsInserted > 0;
            }
            catch (DbException ex)
            {
                // Logger
                // Log DbException Message
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // Logger
                // Log General Exception
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
            return false;
        }

        private static bool IsOverlappingShifts(Shift shift, Shift employeeShift) =>
            (shift.Start - employeeShift.End).TotalMinutes < 0 ||
                (shift.End - employeeShift.Start).TotalMinutes > 0;

        #endregion

        #region Queries

        private const string AssignShiftToEmployeeSql = @"UPDATE Shift 
                                                          SET EmployeeId = @employeeId 
                                                          WHERE Id = @shiftId";

        #endregion
    }
}