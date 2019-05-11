using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Exceptions;
using CloudflareWorkersKv.Client.Models.Namespaces;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests.Namespaces
{
    public class Rename : BaseTest
    {
        [Fact]
        public async Task WhenRenamingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/generic-success-response.json");
            const string title = "newtitle";
            var request = new UpdateRequest
            {
                Title = title
            };

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 200);

                await Client.Namespaces.Rename(NamespaceId, title);

                httpTest
                    .ShouldHaveCalled(NamespacesUrl)
                    .WithVerb(HttpMethod.Put)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Auth-Email", Email)
                    .WithHeader("X-Auth-Key", AuthKey)
                    .WithRequestBody(JsonConvert.SerializeObject(request));
            }
        }

        [Fact]
        public async Task WhenCreatingWithoutId_ArgumentExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Client.Namespaces.Rename(string.Empty, "test"));
        }

        [Fact]
        public async Task WhenCreatingWithoutName_ArgumentExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Client.Namespaces.Rename(NamespaceId, string.Empty));
        }

        [Fact]
        public async Task WhenCreatingNamespaceWithDuplicateName_NamespaceAlreadyExistsException()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/create-namespace-already-exists-response.json");

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 400);

                await Assert.ThrowsAsync<NamespaceAlreadyExistsException>(async () => await Client.Namespaces.Rename(NamespaceId, "title"));
            }
        }

        [Fact]
        public async Task WhenAuthenticationErrorIsReturned_UnauthorizedExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.AuthenticationError, 403);

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.Namespaces.Rename(NamespaceId, "test"));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.Namespaces.Rename(NamespaceId, "test"));
            }
        }

        [Fact]
        public async Task WhenNamespaceNotFoundIsReturned_NamespaceNotFoundExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceNotFound, 404);

                await Assert.ThrowsAsync<NamespaceNotFoundException>(async () => await Client.Namespaces.Rename("unknown", "test"));
            }
        }
    }
}
