using Planday.Schedule.Models.ApiClientModels;

namespace Planday.Schedule.Services.ApiClient
{
    public interface IApiClient
    {
        Task<Employee?> GetEmployeeById(long id);
    }
}