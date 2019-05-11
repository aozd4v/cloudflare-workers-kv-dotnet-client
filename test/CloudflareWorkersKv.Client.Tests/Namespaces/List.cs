using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using Flurl.Http.Testing;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.Namespaces
{
    public class List : BaseTest
    {
        [Fact]
        public async Task WhenListingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/list-namespaces-response.json");

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 200);

                var result = (await Client.Namespaces.List()).ToList();

                Assert.Equal(4, result.Count);
                Assert.NotNull(result.SingleOrDefault(x => x.Title == "test-one"));
                Assert.NotNull(result.SingleOrDefault(x => x.Id == "ta4446c8481a4e3b989bedd542b16g9x"));

                httpTest
                    .ShouldHaveCalled($"{NamespacesUrl}")
                    .WithVerb(HttpMethod.Get)
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

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.Namespaces.List());
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.Namespaces.List());
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.Namespaces.List());
            }
        }
    }
}
