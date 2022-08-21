using FluentAssertions;
using Moq;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Infrastructure.Services;
using Planday.Schedule.Infrastructure.Tests.Helper;
using Planday.Schedule.Queries;
using Planday.Schedule.Services.ApiClient;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Tests.Services
{
    internal class ShiftQueryWithEmployeeTests
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IShiftQueryWithEmployee shiftQueryWithEmployeeService;
        private readonly Mock<ISqliteConnectionFactory> sqliteConnectionFactory;
        private readonly IGetShiftByIdQuery getShiftByIdQuery;
        private readonly Mock<IApiClient> apiClient;

        private SQLiteConnection sqliteConnection;

        #endregion

        #region Ctor

        public ShiftQueryWithEmployeeTests()
        {
            connectionStringProvider = new ConnectionStringProvider("DataSource=:memory:");

            sqliteConnectionFactory = new Mock<ISqliteConnectionFactory>();
            apiClient = new Mock<IApiClient>();

            getShiftByIdQuery = new GetShiftByIdQuery(connectionStringProvider, sqliteConnectionFactory.Object);
            shiftQueryWithEmployeeService = new ShiftQueryWithEmployee(getShiftByIdQuery, apiClient.Object);
        }

        #endregion

        #region Setup

        [SetUp]
        public async Task Setup()
        {
            sqliteConnection = new SQLiteConnection("DataSource=:memory:");
            sqliteConnectionFactory.Setup(scf =>
            scf.GetSqliteConnection(connectionStringProvider.GetConnectiongString()))
                .Returns(sqliteConnection);

            await DatabaseCreationHelper.CreateDatabase(sqliteConnection);
        }

        #endregion

        #region GetEmplyeeByIdQuery Tests

        #region Valid Employee Ids

        [Test]
        [Category("ShiftQueryWithEmployee")]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShiftQueryWithEmployee_CallWithValidData_EmployeeShiftShouldNotBeNull(int shiftId)
        {
            // Arrange
            apiClient.Setup(ac => ac.GetEmployeeById(1))
                .ReturnsAsync(new Schedule.Models.ApiClientModels.Employee
                {
                    Name = "John Doe",
                    Email = "john@doe.com"
                });

            // Act           
            var queryResult = await shiftQueryWithEmployeeService.GetShiftWithEmployeeData(shiftId);

            // Assert
            queryResult?.Should().NotBeNull();
        }

        [Test]
        [Category("ShiftQueryWithEmployee")]
        [TestCase(1, 1, "John Doe", "john@doe.com")]
        [TestCase(2, 2, "Jane Doe", "jane@doe.com")]
        public async Task ShiftQueryWithEmployeeService_CallQueryAsyncWithValidData_ShiftWithEmployeeShouldHaveValidData
            (int shiftId, int employeeId, string employeeName, string employeeEmail)
        {
            // Arrange
            apiClient.Setup(ac => ac.GetEmployeeById(employeeId))
                .ReturnsAsync(new Schedule.Models.ApiClientModels.Employee
                {
                    Name = employeeName,
                    Email = employeeEmail
                });

            // Act           
            var queryResult = await shiftQueryWithEmployeeService.GetShiftWithEmployeeData(shiftId);

            // Assert
            queryResult?.EmployeeId.Should().Be(employeeId);
            queryResult?.EmployeeName.Should().Be(employeeName);
            queryResult?.EmployeeEmail.Should().Be(employeeEmail);
        }

        #endregion

        #endregion
    }
}
