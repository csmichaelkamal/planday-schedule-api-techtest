using Microsoft.AspNetCore.Mvc;
using Planday.Schedule.Api.Models.Request;
using Planday.Schedule.Commands;
using Planday.Schedule.Infrastructure.Responses;
using Planday.Schedule.Infrastructure.Services;
using System.Net;

namespace Planday.Schedule.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShiftController : ControllerBase
    {
        #region Members

        private readonly IShiftQueryWithEmployee shiftQueryWithEmployee;
        private readonly ICreateOpenShiftCommand createOpenShiftCommand;
        private readonly IAssignShiftToEmployeeCommand assignShiftToEmployeeCommand;
        private readonly ILogger<ShiftController> logger;

        #endregion

        #region Ctor

        public ShiftController(IShiftQueryWithEmployee shiftQueryWithEmployee,
            ICreateOpenShiftCommand createOpenShiftCommand,
            IAssignShiftToEmployeeCommand assignShiftToEmployeeCommand,
            ILogger<ShiftController> logger)
        {
            this.shiftQueryWithEmployee = shiftQueryWithEmployee;
            this.createOpenShiftCommand = createOpenShiftCommand;
            this.assignShiftToEmployeeCommand = assignShiftToEmployeeCommand;
            this.logger = logger;
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
                logger.LogError($"shift with id: {id} couldn't be found!");
                return NotFound(new NotFoundResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"Couldn't find a shift with Id {id}"
                });
            }

            logger.LogInformation($"Shift with Id {id} has been found!");
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
                logger.LogCritical("Error while adding the open shift, please try again!");
                return BadRequest();
            }

            logger.LogInformation($"Open Shift was added successfully, Open Shift Id is {insertedId}!");
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
                logger.LogError($"Cannot Assign the Shift with Id = {assignShiftToEmployeeRequestModel.ShiftId} " +
                    $"to the Employee with Id = {assignShiftToEmployeeRequestModel.EmployeeId}");
                return BadRequest();
            }

            logger.LogInformation($"Shift with Id {assignShiftToEmployeeRequestModel.ShiftId} has been successfully assigned to" +
                $"Employee with Id {assignShiftToEmployeeRequestModel.EmployeeId}");
            return NoContent();
        }

        #endregion

        #endregion
    }
}

