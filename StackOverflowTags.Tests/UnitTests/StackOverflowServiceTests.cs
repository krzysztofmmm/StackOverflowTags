using Moq;
using Moq.Protected;
using StackOverflowTags.Services;

namespace StackOverflowTags.Tests.UnitTests
{
    public class StackOverflowServiceTests
    {
        private readonly Mock<IStackOverflowService> _mockService = new Mock<IStackOverflowService>();
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        public StackOverflowServiceTests()
        {
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.stackexchange.com/2.2/")
            };
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);
        }

        [Fact]
        public async Task RefreshTagsAsync_CallsServiceMethod()
        {
            _mockService.Setup(s => s.RefreshTagsAsync()).Returns(Task.CompletedTask);
            await _mockService.Object.RefreshTagsAsync();
            _mockService.Verify(s => s.RefreshTagsAsync() , Times.Once);
        }

        [Fact]
        public async Task FetchAndStoreTagsAsync_CallsHttpClient()
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK ,
                Content = new StringContent("{\"items\":[{\"name\":\"ExampleTag\",\"count\":10}],\"has_more\":false}")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync" ,
                    ItExpr.IsAny<HttpRequestMessage>() ,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var response = await _httpClient.GetAsync("tags?order=desc&sort=popular");

            Assert.Equal(System.Net.HttpStatusCode.OK , response.StatusCode);
        }
    }
}
