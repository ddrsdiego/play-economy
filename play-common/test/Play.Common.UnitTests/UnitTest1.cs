namespace Play.Common.UnitTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Xunit;

    public class UnitTest1
    {
        private readonly ILoggerFactory _logger;
        
        public UnitTest1()
        {
            _logger = Substitute.For<ILoggerFactory>();
        }
        
        [Fact]
        public async Task Test1()
        {
            var sut = new RegisterNewCustomerUseCase(_logger);
            
            var request = new RegisterNewCustomerRequest();
            var response = await sut.SendAsync(request);

            response.RequestId.Should().Be(request.RequestId);
            response.StatusCode.Should().Be(StatusCodes.Status201Created);
        }
    }

    
}