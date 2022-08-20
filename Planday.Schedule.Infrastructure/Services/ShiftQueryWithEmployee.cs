using Planday.Schedule.Queries;
using Planday.Schedule.Services.ApiClient;

namespace Planday.Schedule.Infrastructure.Services
{
    public class ShiftQueryWithEmployee : IShiftQueryWithEmployee
    {
        #region Private Members

        private readonly IGetShiftByIdQuery getShiftByIdQuery;
        private readonly IApiClient apiClient;

        #endregion

        #region Ctor

        public ShiftQueryWithEmployee(IGetShiftByIdQuery getShiftByIdQuery, IApiClient apiClient)
        {
            this.getShiftByIdQuery = getShiftByIdQuery;
            this.apiClient = apiClient;
        }

        #endregion

        public async Task<ShiftEmployee> GetShiftWithEmployeeData(long shiftId)
        {
            var shift = await getShiftByIdQuery.QueryAsync(shiftId);

            if (shift is null)
            {
                // Log Shift with Id = shiftId couldn't be found
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (shift.EmployeeId.HasValue)
            {
                var employeeData = await apiClient.GetEmployeeById(shift.EmployeeId.Value);

                if (employeeData is not null)
                {
                    var shiftWithEmployeeData = new ShiftEmployee(shift.Id, shift.EmployeeId.Value, shift.Start,
                        shift.End, employeeData.Name, employeeData.Email);
                    return shiftWithEmployeeData;
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var openShift = new ShiftEmployee(shift.Id, new long(), shift.Start,
                   shift.End, string.Empty, string.Empty);

            return openShift;
        }
    }
}
