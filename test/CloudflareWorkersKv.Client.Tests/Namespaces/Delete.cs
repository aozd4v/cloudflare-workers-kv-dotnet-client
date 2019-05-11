using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using Flurl.Http.Testing;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.Namespaces
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

                await Client.Namespaces.Delete(NamespaceId);

                httpTest
                    .ShouldHaveCalled(NamespaceUrl)
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

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.Namespaces.Delete(NamespaceId));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.Namespaces.Delete(NamespaceId));
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.Namespaces.Delete(NamespaceId));
            }
        }
    }
}
