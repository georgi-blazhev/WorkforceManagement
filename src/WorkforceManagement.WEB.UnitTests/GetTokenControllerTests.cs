using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WorkforceManagement.WEB.Controllers;
using Xunit;

namespace WorkforceManagement.WEB.UnitTests
{
    public class GetTokenControllerTests : BaseWebTest
    {
        [Fact]
        public async void GetToken_Default_CallsPostAsync()
        {
            // arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var sut = new GetTokenController(UserService.Object, client);

            // act
            var result = await sut.GetToken("fake-username-or-email", "fake-password");

            // assert
            Assert.Equal("", result);
        }
    }
}
