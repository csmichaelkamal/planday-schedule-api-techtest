using Newtonsoft.Json;
using Planday.Schedule.Models.ApiClientModels;
using System.Net;

namespace Planday.Schedule.Services.ApiClient
{
    /// <summary>
    /// Service for making Http requests to the endpoint configured in the Main Project or Settings
    /// </summary>
    public class ApiClient : IApiClient
    {
        #region Private Mebmers

        private readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public ApiClient(HttpClient httpClient, string baseUrl, string apiSecret)
        {
            _ = string.IsNullOrEmpty(baseUrl) ? throw new ArgumentNullException(nameof(baseUrl)) : baseUrl;
            _ = string.IsNullOrEmpty(apiSecret) ? throw new ArgumentNullException(nameof(apiSecret)) : apiSecret;

            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Add(nameof(HttpRequestHeader.Authorization), apiSecret);
        }

        #endregion

        /// <summary>
        /// Making a Http call to the Employee Service to retrieve the Employee Data based on the given <paramref name="id"/>, if exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Employee?> GetEmployeeById(long id)
        {
            if (id <= 0)
            {
                return null;
            }

            try
            {
                var employeeServiceResponse = await _httpClient.GetAsync($"/employee/{id}");

                if (employeeServiceResponse is null || !employeeServiceResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                var employeeResponseString = await employeeServiceResponse.Content.ReadAsStringAsync();

                var serializedEmployeeResponse = JsonConvert.DeserializeObject<Employee>(employeeResponseString);

                return serializedEmployeeResponse;
            }
            catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.NotFound)
            {
                // Log Exception
            }
            catch (Exception ex)
            {
                // Log Error if needed
                // Apply retry policy
            }
            finally
            {
                _httpClient.Dispose();
            }

            return null;
        }
    }
}
