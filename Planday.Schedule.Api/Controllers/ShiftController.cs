using Microsoft.AspNetCore.Mvc;
using Planday.Schedule.Infrastructure.Responses;
using Planday.Schedule.Queries;
using System.Net;

namespace Planday.Schedule.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController : ControllerBase
    {
        #region Members

        private readonly IGetShiftByIdQuery _getShiftByIdQuery;

        #endregion

        #region Ctor

        public ShiftController(IGetShiftByIdQuery getShiftByIdQuery)
        {
            _getShiftByIdQuery = getShiftByIdQuery;
        }

        #endregion

        #region Public End-Points

        /// <summary>
        /// Get Shift by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Shift by the supplied id if found, otherwise returns NotFound (404)</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Shift), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
        [ProducesErrorResponseType(typeof(HttpStatusCode))]
        public async Task<IActionResult> GetShiftById(long id)
        {
            var shift = await _getShiftByIdQuery.QueryAsync(id);

            if (shift is null)
            {
                // logger Error into the logger

                return NotFound(new NotFoundResponse {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"Couldn't find a shift with Id {id}"
                });
            }

            return Ok(shift);
        }

        #endregion
    }
}

