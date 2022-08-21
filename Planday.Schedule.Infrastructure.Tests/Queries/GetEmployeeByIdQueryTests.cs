using Moq;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Infrastructure.Tests.Services;
using Planday.Schedule.Queries;
using System.Data.SQLite;
using FluentAssertions;

namespace Planday.Schedule.Infrastructure.Tests.Queries
{
    public class GetEmployeeByIdQueryTests
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly Mock<ISqliteConnectionFactory> sqliteConnectionFactory;
        private readonly IGetEmployeeByIdQuery getEmployeeByIdQuery;

        private SQLiteConnection sqliteConnection;

        private static readonly int[] ValidEmployeeIds = new int[] { 1, 2 };
        private static readonly int[] InvalidEmployeeIds = new int[] { 3, 4 };

        #endregion

        #region Ctor

        public GetEmployeeByIdQueryTests()
        {
            connectionStringProvider = new ConnectionStringProvider("DataSource=:memory:");
            sqliteConnectionFactory = new Mock<ISqliteConnectionFactory>();
            getEmployeeByIdQuery = new GetEmployeeByIdQuery(connectionStringProvider,
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
        [Category("GetEmployeeByIdQuery_QueryAsync_Valid")]
        [TestCaseSource(nameof(ValidEmployeeIds))]
        public async Task GetEmployeeByIdQuery_CallQueryAsyncWithValidId_EmployeeShouldNotBeNull(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeByIdQuery.QueryAsync(id);

            // Assert
            queryResult.Should().NotBeNull();
        }

        [Test]
        [Category("GetEmployeeByIdQuery_QueryAsync_Valid")]
        [TestCaseSource(nameof(ValidEmployeeIds))]
        public async Task GetEmployeeByIdQuery_CallQueryAsyncWithValidId_ShouldReturnValidEmployee(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeByIdQuery.QueryAsync(id);

            // Assert
            queryResult?.Id.Should().Be(id);
            queryResult?.Name.Should().NotBeNullOrEmpty();

            queryResult.Should().BeOfType(typeof(Employee));
        }

        #endregion

        #region Invalid Employee Ids

        [Test]
        [Category("GetEmployeeByIdQuery_QueryAsync_Invalid")]
        [TestCaseSource(nameof(InvalidEmployeeIds))]
        public async Task GetEmployeeByIdQuery_CallQueryAsyncWithInvalidId_EmployeeShouldBeNull(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getEmployeeByIdQuery.QueryAsync(id);

            // Assert
            queryResult.Should().BeNull();
        }

        #endregion

        #endregion
    }
}
