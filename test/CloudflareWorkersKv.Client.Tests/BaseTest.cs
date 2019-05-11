namespace CloudflareWorkersKv.Client.Tests
{
    public class BaseTest
    {
        protected readonly string Email;
        protected readonly string AuthKey;
        protected readonly ICloudflareWorkersKvClient<SampleResponse> Client;
        protected readonly string SampleKey;
        protected readonly string NamespacesUrl;
        protected readonly string NamespaceUrl;
        protected readonly string ValidCloudflareWorkersKvUrl;
        protected readonly string SampleResponse;
        protected readonly string NamespaceId;

        public BaseTest()
        {
            Email = "some@email.com";
            AuthKey = "somekey";
            const string accountId = "account";
            NamespaceId = "ta4446c8481a4e3b989bedd542b16g9x";
            Client = new CloudflareWorkersKvClient<SampleResponse>(Email, AuthKey, accountId, NamespaceId);
            SampleKey = "sample";
            NamespacesUrl = $"https://api.cloudflare.com/client/v4/accounts/{accountId}/storage/kv/namespaces";
            NamespaceUrl = $"https://api.cloudflare.com/client/v4/accounts/{accountId}/storage/kv/namespaces/{NamespaceId}";
            ValidCloudflareWorkersKvUrl = $"{NamespaceUrl}/values/{SampleKey}";
            SampleResponse = @"
                {
                  ""name"": ""test"",
                  ""cost"": ""1.99""
                }";
        }
    }
}
