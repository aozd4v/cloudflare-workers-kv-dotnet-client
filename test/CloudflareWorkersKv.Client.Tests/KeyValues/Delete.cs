using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using Flurl.Http.Testing;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.KeyValues
{
    public class Delete : BaseTest
    {
        [Fact]
        public async Task WhenDeletingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/generic-success-response.json");

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 200);

                await Client.KeyValues.Delete(SampleKey);

                httpTest
                    .ShouldHaveCalled(ValidCloudflareWorkersKvUrl)
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Auth-Email", Email)
                    .WithHeader("X-Auth-Key", AuthKey);
            }
        }

        [Fact]
        public async Task WhenAuthenticationErrorIsReturned_UnauthorizedExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.AuthenticationError, 403);

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.KeyValues.Delete(SampleKey));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.KeyValues.Delete(SampleKey));
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.KeyValues.Delete(SampleKey));
            }
        }
    }
}
