namespace CloudflareWorkersKv.Client.Tests
{
    public class BaseTest
    {
        protected readonly string Email;
        protected readonly string AuthKey;
        protected readonly ICloudflareWorkersKvClient Client;
        protected readonly string SampleKey;
        protected readonly string NamespacesUrl;
        protected readonly string ValidCloudflareWorkersKvUrl;
        protected readonly string SampleResponse;

        public BaseTest()
        {
            Email = "some@email.com";
            AuthKey = "somekey";
            const string accountId = "account";
            const string namespaceId = "namespace";
            Client = new CloudflareWorkersKvClient(Email, AuthKey, accountId, namespaceId);
            SampleKey = "sample";
            NamespacesUrl = $"https://api.cloudflare.com/client/v4/accounts/{accountId}/storage/kv/namespaces/{namespaceId}";
            ValidCloudflareWorkersKvUrl = $"{NamespacesUrl}/values/{SampleKey}";
            SampleResponse = @"
                {
                  ""name"": ""test"",
                  ""cost"": ""1.99""
                }";
        }
    }
}
