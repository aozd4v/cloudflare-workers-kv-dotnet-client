using CloudflareWorkersKv.Client.Services;

namespace CloudflareWorkersKv.Client
{
    public class CloudflareWorkersKvClient<T> : ICloudflareWorkersKvClient<T>
    {
        private readonly string _baseUrl = "https://api.cloudflare.com/client/v4/accounts";

        public CloudflareWorkersKvClient(IKeyValueService<T> keyValueService)
        {
            KeyValues = keyValueService;
        }

        public CloudflareWorkersKvClient(string email,
                                         string authKey,
                                         string accountId,
                                         string namespaceId)
        {
            var headers = new
            {
                X_Auth_Email = email,
                X_Auth_Key = authKey,
                Content_Type = "application/json"
            };

            _baseUrl = $"{_baseUrl}/{accountId}";
            var namespacesUrl = $"{_baseUrl}/storage/kv/namespaces";
            KeyValues = new KeyValueService<T>($"{namespacesUrl}/{namespaceId}", headers);
            Namespaces = new NamespaceService(namespacesUrl, headers);
        }

        public IKeyValueService<T> KeyValues { get; }
        public INamespaceService Namespaces { get; }
    }
}
