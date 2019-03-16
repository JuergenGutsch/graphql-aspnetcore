using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQlDemo;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GraphQl.AspNetCore.IntegrationTest
{
    public class GraphQLMiddlewareIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GraphQLMiddlewareIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Tests the valid and invalid <see cref="HttpMethod"/> for the <see cref="GraphQlMiddleware"/>.
        /// </summary>
        [Theory()]
        [MemberData(nameof(GetHttpReuqestMessages))]
        public async Task HttpMethodsTest((HttpRequestMessage HttpRequestMessage, HttpStatusCode HttpMethod) param)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.SendAsync(param.HttpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

            // Assert
            Assert.Equal(param.HttpMethod, response.StatusCode);
        }

        public static IEnumerable<object[]> GetHttpReuqestMessages()
        {
            return new HttpRequestMessageTheoryData();
        }

        private class HttpRequestMessageTheoryData : TheoryData<(HttpRequestMessage, HttpStatusCode)>
        {
            private const string uri = "/graphql";

            public HttpRequestMessageTheoryData()
            {
                Add((new HttpRequestMessage(HttpMethod.Head, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Options, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Patch, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Delete, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Put, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Trace, uri), HttpStatusCode.MethodNotAllowed));
                Add((new HttpRequestMessage(HttpMethod.Get, uri), HttpStatusCode.OK));
                Add((new HttpRequestMessage(HttpMethod.Post, uri), HttpStatusCode.OK));
            }
        }
    }
}
