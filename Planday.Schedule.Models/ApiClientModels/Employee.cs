using Newtonsoft.Json;

namespace Planday.Schedule.Models.ApiClientModels
{
    /// <summary>
    /// Model represents the employee object returned by an API
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Employee Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Employee Email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}