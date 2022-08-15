using FluentAssertions;
using Moq;
using Planday.Schedule.Infrastructure.Providers.Interfaces;
using Planday.Schedule.Queries;

namespace Planday.Schedule.Infrastructure.Tests.Queries
{
    public class GetShiftByIdQueryTests
    {
        #region Private Members

        private readonly Mock<IConnectionStringProvider> connectionStringProvider;
        private readonly IGetShiftByIdQuery getShiftByIdQuery;

        #endregion

        #region Ctor

        public GetShiftByIdQueryTests(Mock<IConnectionStringProvider> connectionStringProvider,
            IGetShiftByIdQuery getShiftByIdQuery)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.getShiftByIdQuery = getShiftByIdQuery;
        }

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            connectionStringProvider
                .Setup(csp => csp.GetConnectiongString())
                .Returns("ConnectionString");
        }

        #endregion

        #region MyRegion

        #endregion

        [Test]
        [Category("GetShiftByIdQuery_QueryAsync")]
        public async Task GetShiftByIdQuery_QueryAsync_Id()
        {
            var queryResult = await getShiftByIdQuery.QueryAsync(1);

            queryResult.Should().NotBeNull();
        }
    }
}