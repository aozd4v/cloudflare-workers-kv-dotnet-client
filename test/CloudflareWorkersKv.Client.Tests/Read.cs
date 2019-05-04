using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests
{
    public class Read : BaseTest
    {
        [Fact]
        public async Task WhenReadingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(SampleResponse, 200);

                var result = await Client.Read(SampleKey);

                Assert.Equal("test", result.Name);
                Assert.Equal(1.99m, result.Cost);

                httpTest
                    .ShouldHaveCalled(ValidCloudflareWorkersKvUrl)
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Auth-Email", Email)
                    .WithHeader("X-Auth-Key", AuthKey);
            }
        }

        [Fact]
        public async Task WhenReadingSuccessfully_ButResponseCantBeDeserialized_ThenExceptionIsThrown()
        {
            var client = new CloudflareWorkersKvClient<int>(string.Empty, string.Empty, string.Empty,
                string.Empty);

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(SampleResponse, 200);

                await Assert.ThrowsAsync<JsonDeserializationException>(async () => await client.Read(SampleKey));
            }
        }

        [Fact]
        public async Task WhenAuthenticationErrorIsReturned_UnauthorizedExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.AuthenticationError, 403);

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.Read(SampleKey));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.Read(SampleKey));
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.Read(SampleKey));
            }
        }

        [Fact]
        public async Task WhenKeyNotFoundIsReturned_NullIsReturned()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.KeyNotFoundError, 404);

                var result = await Client.Read(SampleKey);

                Assert.Null(result);
            }
        }
    }
}
