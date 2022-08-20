using Microsoft.AspNetCore.Mvc;
using Planday.Schedule.Api.Models.Request;
using Planday.Schedule.Commands;
using Planday.Schedule.Infrastructure.Responses;
using Planday.Schedule.Infrastructure.Services;
using System.Net;

namespace Planday.Schedule.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        #region Members

        private readonly IShiftQueryWithEmployee shiftQueryWithEmployee;
        private readonly ICreateOpenShiftCommand createOpenShiftCommand;
        private readonly IAssignShiftToEmployeeCommand assignShiftToEmployeeCommand;

        #endregion

        #region Ctor

        public ShiftController(IShiftQueryWithEmployee shiftQueryWithEmployee,
            ICreateOpenShiftCommand createOpenShiftCommand,
            IAssignShiftToEmployeeCommand assignShiftToEmployeeCommand)
        {
            this.shiftQueryWithEmployee = shiftQueryWithEmployee;
            this.createOpenShiftCommand = createOpenShiftCommand;
            this.assignShiftToEmployeeCommand = assignShiftToEmployeeCommand;
        }

        #endregion

        #region Public End-Points

        #region Get Shift By Id

        /// <summary>
        /// Get Shift by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Shift by the supplied id if found, otherwise returns NotFound (404)</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShiftEmployee), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetShiftById(long id)
        {
            var shift = await shiftQueryWithEmployee.GetShiftWithEmployeeData(id);

            if (shift is null)
            {
                // logger Error into the logger
                return NotFound(new NotFoundResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"Couldn't find a shift with Id {id}"
                });
            }

            return Ok(shift);
        }

        #endregion

        #region Add Open Shift

        /// <summary>
        /// Add An Open Shift, An open shift is a Shift without an employee assigned
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If the Open Shift created successfully, returns HTTP Status Code Created (201), otherwise returns Bad Request (400)</returns>
        [HttpPost("openshift")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOpenShift(OpenShiftRequestModel openShiftRequestModel)
        {
            var insertedId = await createOpenShiftCommand.CreateOpenShiftAsync(openShiftRequestModel.StartDate,
                openShiftRequestModel.EndDate);

            if (insertedId is 0)
            {
                // logger Error into the logger
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetShiftById), new { id = insertedId }, insertedId);
        }

        #endregion

        #region Assign A Shift to an Employee

        [HttpPost("assign")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignShiftToEmployee(AssignShiftToEmployeeRequestModel assignShiftToEmployeeRequestModel)
        {
            var assingStatus = await assignShiftToEmployeeCommand.
                AssignShiftToEmployeeAsync(assignShiftToEmployeeRequestModel.ShiftId, assignShiftToEmployeeRequestModel.EmployeeId);

            if (!assingStatus)
            {
                // logger Error into the logger
                // Cannot Assign the Shift with Id = ShiftId to the Employee with Id = EmployeeId
                return BadRequest();
            }

            return NoContent();
        }

        #endregion

        #endregion
    }
}

