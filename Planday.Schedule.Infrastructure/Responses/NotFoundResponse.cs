namespace Planday.Schedule.Infrastructure.Responses
{
    public class NotFoundResponse
    {
        /// <summary>
        /// Http Status Code for the error
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error Message indicate what is the problem with the request
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
