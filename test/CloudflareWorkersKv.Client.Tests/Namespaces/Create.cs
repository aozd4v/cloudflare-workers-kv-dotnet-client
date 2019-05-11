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
    public class Create : BaseTest
    {
        [Fact]
        public async Task WhenCreatingSuccessfully_RequestShouldBeConstructedCorrectly()
        {
            const string id = "5z2l6954b0a544afa4b998193185817b";
            const string title = "test";
            var response = new CreateResponse
            {
                Result = new CreateResult
                {
                    Id = id,
                    Title = title
                }
            };

            var request = new CreateRequest
            {
                Title = title
            };

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(JsonConvert.SerializeObject(response), 200);

                var result = await Client.Namespaces.Create(title);

                Assert.Equal(id, result);

                httpTest
                    .ShouldHaveCalled(NamespacesUrl)
                    .WithVerb(HttpMethod.Post)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Auth-Email", Email)
                    .WithHeader("X-Auth-Key", AuthKey)
                    .WithRequestBody(JsonConvert.SerializeObject(request));
            }
        }

        [Fact]
        public async Task WhenCreatingWithoutName_ArgumentExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Client.Namespaces.Create(string.Empty));
        }

        [Fact]
        public async Task WhenCreatingNamespaceWithDuplicateName_NamespaceAlreadyExistsException()
        {
            var response = await File.ReadAllTextAsync("./Fixtures/create-namespace-already-exists-response.json");

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(response, 400);

                await Assert.ThrowsAsync<NamespaceAlreadyExistsException>(async () => await Client.Namespaces.Create("test"));
            }
        }

        [Fact]
        public async Task WhenAuthenticationErrorIsReturned_UnauthorizedExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.AuthenticationError, 403);

                await Assert.ThrowsAsync<UnauthorizedException>(async () => await Client.Namespaces.Create("test"));
            }
        }

        [Fact]
        public async Task WhenNamespaceFormattingErrorIsReturned_NamespaceFormattingExceptionIsThrown()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(CloudflareErrors.NamespaceFormattingError, 400);

                await Assert.ThrowsAsync<NamespaceFormattingException>(async () => await Client.Namespaces.Create("test"));
            }
        }
    }
}
