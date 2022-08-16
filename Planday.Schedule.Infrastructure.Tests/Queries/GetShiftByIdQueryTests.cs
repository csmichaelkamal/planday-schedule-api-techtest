#region Usings

using Dapper;
using FluentAssertions;
using Moq;
using Planday.Schedule.Infrastructure.Factories.Interfaces;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Infrastructure.Queries;
using Planday.Schedule.Queries;
using System.Data;
using System.Data.SQLite;

#endregion

namespace Planday.Schedule.Infrastructure.Tests.Queries
{
    public class GetShiftByIdQueryTests
    {
        #region Private Members

        private readonly Mock<IConnectionStringProvider> connectionStringProvider;
        private readonly Mock<ISqliteConnectionFactory> sqliteConnectionFactory;
        private readonly IGetShiftByIdQuery getShiftByIdQuery;
        private SQLiteConnection sqliteConnection;

        #endregion

        #region Ctor

        public GetShiftByIdQueryTests()
        {
            connectionStringProvider = new Mock<IConnectionStringProvider>();
            sqliteConnectionFactory = new Mock<ISqliteConnectionFactory>();
            getShiftByIdQuery = new GetShiftByIdQuery(connectionStringProvider.Object,
                sqliteConnectionFactory.Object);
        }

        #endregion

        #region Setup

        [SetUp]
        public async Task Setup()
        {
            connectionStringProvider
                .Setup(csp => csp.GetConnectiongString())
                .Returns("DataSource=:memory:");

            sqliteConnection = new SQLiteConnection();

            sqliteConnectionFactory.Setup(scf =>
            scf.GetSqliteConnection(connectionStringProvider.Object.GetConnectiongString()))
                .Returns(sqliteConnection);
        }

        #endregion

        #region GetShiftByIdQuer Tests

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync")]
        public async Task GetShiftByIdQuery_QueryAsync_Id()
        {
            var queryResult = await getShiftByIdQuery.QueryAsync(1);

            queryResult.Should().NotBeNull();
        }

        #endregion
    }

    public class ShiftDto
    {
        public int Id { get; set; }

    }
}