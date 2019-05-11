using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using Flurl.Http.Testing;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.KeyValues
{
    public class List : BaseTest
    {
        [Fact]
        public async Task WhenListingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/list-keys-response.json");

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 200);

                const string cursor = "somecursor";
                var result = await Client.KeyValues.List(cursor);
                var count = result.Keys.Distinct().Count();

                Assert.Equal(1000, count);
                Assert.NotNull(result.Keys.SingleOrDefault(x => x == "816"));
                Assert.NotNull(result.Keys.SingleOrDefault(x => x == "78"));

                httpTest
                    .ShouldHaveCalled($"{NamespaceUrl}/keys?cursor={cursor}")
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

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.KeyValues.List());
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.KeyValues.List());
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.KeyValues.List());
            }
        }
    }
}
