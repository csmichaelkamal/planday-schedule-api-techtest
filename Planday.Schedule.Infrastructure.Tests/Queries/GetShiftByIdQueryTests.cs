#region Usings

using FluentAssertions;
using Moq;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Infrastructure.Tests.Helper;
using Planday.Schedule.Queries;
using System.Data.SQLite;

#endregion

namespace Planday.Schedule.Infrastructure.Tests.Queries
{
    public class GetShiftByIdQueryTests
    {
        #region Private Members

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly Mock<ISqliteConnectionFactory> sqliteConnectionFactory;
        private readonly IGetShiftByIdQuery getShiftByIdQuery;

        private SQLiteConnection sqliteConnection;

        private static readonly int[] ValidShiftIds = new int[] { 1, 2 };
        private static readonly int[] InvalidShiftIds = new int[] { 3, 4 };

        #endregion

        #region Ctor

        public GetShiftByIdQueryTests()
        {
            connectionStringProvider = new ConnectionStringProvider("DataSource=:memory:");
            sqliteConnectionFactory = new Mock<ISqliteConnectionFactory>();
            getShiftByIdQuery = new GetShiftByIdQuery(connectionStringProvider,
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

        #region GetShiftByIdQuery Tests

        #region Valid Shift Ids

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync_Valid")]
        [TestCaseSource(nameof(ValidShiftIds))]
        public async Task GetShiftByIdQuery_CallQueryAsyncWithValidId_ShiftShouldNotBeNull(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getShiftByIdQuery.QueryAsync(id);

            // Assert
            queryResult.Should().NotBeNull();
        }

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync_Valid")]
        [TestCaseSource(nameof(ValidShiftIds))]
        public async Task GetShiftByIdQuery_CallQueryAsyncWithValidId_ShouldReturnValidShift(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getShiftByIdQuery.QueryAsync(id);

            // Assert
            queryResult?.Id.Should().Be(id);
            queryResult?.Start.Should().NotBe(null);
            queryResult?.End.Should().NotBe(null);

            queryResult.Should().BeOfType(typeof(Shift));
        }

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync_Valid")]
        [TestCaseSource(nameof(ValidShiftIds))]
        public async Task GetShiftByIdQuery_CallQueryAsyncWithValidId_StartAndEndDatesShouldBeOnSameDay(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getShiftByIdQuery.QueryAsync(id);

            // Assert
            queryResult?.Start.ToShortDateString().Should().Be(queryResult?.End.ToShortDateString());
        }

        #endregion

        #region Invalid Shift Ids

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync_Invalid")]
        [TestCaseSource(nameof(InvalidShiftIds))]
        public async Task GetShiftByIdQuery_CallQueryAsyncWithInvalidId_ShiftShouldBeNull(int id)
        {
            // Arrange
            // Act           
            var queryResult = await getShiftByIdQuery.QueryAsync(id);

            // Assert
            queryResult.Should().BeNull();
        }

        #endregion

        #endregion
    }
}