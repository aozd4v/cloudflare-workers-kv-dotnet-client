using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.KeyValues
{
    public class Write : BaseTest
    {
        private readonly SampleResponse _sample;

        public Write()
        {
            _sample = new SampleResponse
            {
                Cost = 1.99m,
                Name = "test"
            };
        }

        [Fact]
        public async Task WhenWritingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(SampleResponse, 200);

                await Client.KeyValues.Write(SampleKey, _sample);

                httpTest
                    .ShouldHaveCalled(ValidCloudflareWorkersKvUrl)
                    .WithVerb(HttpMethod.Put)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Auth-Email", Email)
                    .WithHeader("X-Auth-Key", AuthKey)
                    .WithRequestBody(JsonConvert.SerializeObject(_sample));
            }
        }

        [Fact]
        public async Task WhenAuthenticationErrorIsReturned_UnauthorizedExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.AuthenticationError, 403);

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.KeyValues.Write(SampleKey, _sample));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.KeyValues.Write(SampleKey, _sample));
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.KeyValues.Write(SampleKey, _sample));
            }
        }
    }
}
