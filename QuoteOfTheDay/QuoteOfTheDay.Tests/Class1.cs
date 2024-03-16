// FILEPATH: /c:/git/Whoright/GitHubCopilotWTH/ProgramTests.cs
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class ProgramTests
{
    [Theory]
    [InlineData("random", "https://zenquotes.io/api/random")]
    [InlineData("day", "https://zenquotes.io/api/today")]
    public async Task Main_CallsCorrectEndpoint(string quoteType, string expectedEndpoint)
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req =>
                 req.Method == HttpMethod.Get
                 && req.RequestUri.ToString() == expectedEndpoint),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(JsonConvert.SerializeObject(new[] { new QuoteResponse { q = "quote", a = "author" } })),
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://zenquotes.io/api/"),
        };

        // Act
        await Program.GetQuote(quoteType, httpClient);

        // Assert
        handlerMock.Protected().Verify(
           "SendAsync",
           Times.Exactly(1),
           ItExpr.Is<HttpRequestMessage>(req =>
              req.Method == HttpMethod.Get
              && req.RequestUri.ToString() == expectedEndpoint),
           ItExpr.IsAny<CancellationToken>()
        );
    }
}