using FluentAssertions;
using Moq;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Infrastructure.Tests.Services;
using Planday.Schedule.Queries;
using System.Data.SQLite;

namespace Planday.Schedule.Infrastructure.Tests.Queries
{
    internal class GetEmployeeShiftQueryTests
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly Mock<ISqliteConnectionFactory> sqliteConnectionFactory;
        private readonly IGetEmployeeShiftQuery getEmployeeShiftQuery;

        private SQLiteConnection sqliteConnection;

        #endregion

        #region Ctor

        public GetEmployeeShiftQueryTests()
        {
            connectionStringProvider = new ConnectionStringProvider("DataSource=:memory:");
            sqliteConnectionFactory = new Mock<ISqliteConnectionFactory>();
            getEmployeeShiftQuery = new GetEmployeeShiftQuery(connectionStringProvider,
                sqliteConnectionFactory.Object);
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
        [Category("GetEmployeeShiftQuery_QueryAsync_Valid")]
        [TestCase(1, "2022-08-19 12:00:00.000")]
        [TestCase(2, "2022-08-21 14:00:00.000")]
        public async Task GetEmployeeShiftQuery_CallQueryAsyncWithValidData_ShiftWithEmployeeShouldNotBeNull(int id, string startDateTime)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeShiftQuery.QueryAsync(id, startDateTime);

            // Assert
            queryResult?.Should().NotBeNull();
        }

        [Test]
        [Category("GetEmployeeShiftQuery_QueryAsync_Valid")]
        [TestCase(1, "2022-08-19 12:00:00.000")]
        [TestCase(2, "2022-08-21 14:00:00.000")]
        public async Task GetEmployeeShiftQuery_CallQueryAsyncWithValidData_ShiftWithEmployeeShouldBeReturned(int id, string startDateTime)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeShiftQuery.QueryAsync(id, startDateTime);

            // Assert
            queryResult?.Id.Should().Be(id);
            queryResult?.Start.ToShortDateString().Should().Be(startDateTime);
        }

        #endregion

        #region Invalid Employee Ids

        [Test]
        [Category("GetEmployeeByIdQuery_QueryAsync_Invalid")]
        [TestCase(3, "2022-09-12 12:00:00.000")]
        [TestCase(4, "2022-09-04 14:00:00.000")]
        public async Task GetEmployeeByIdQuery_CallQueryAsyncWithInvalidId_EmployeeShouldBeNull(int id, string startDateTime)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeShiftQuery.QueryAsync(id, startDateTime);

            // Assert
            queryResult.Should().BeNull();
        }

        #endregion

        #endregion
    }
}
