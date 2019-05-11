using System;
using CloudflareWorkersKv.Client.Services;

namespace CloudflareWorkersKv.Client
{
    public class CloudflareWorkersKvClient<T> : ICloudflareWorkersKvClient<T>
    {
        private readonly string _baseUrl = "https://api.cloudflare.com/client/v4/accounts";
        private readonly string _namespaceId;
        private readonly IKeyValueService<T> _keyValueService;

        public CloudflareWorkersKvClient(IKeyValueService<T> keyValueService,
                                         INamespaceService namespaceService)
        {
            _keyValueService = keyValueService;
            Namespaces = namespaceService;
        }

        public CloudflareWorkersKvClient(string email,
                                         string key,
                                         string accountId,
                                         string namespaceId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("email is required");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key is required");
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                throw new ArgumentException("account id is required");
            }

            var headers = new
            {
                X_Auth_Email = email,
                X_Auth_Key = key,
                Content_Type = "application/json"
            };

            _namespaceId = namespaceId;
            _baseUrl = $"{_baseUrl}/{accountId}";
            var namespacesUrl = $"{_baseUrl}/storage/kv/namespaces";
            _keyValueService = new KeyValueService<T>($"{namespacesUrl}/{_namespaceId}", headers);
            Namespaces = new NamespaceService(namespacesUrl, headers);
        }

        public IKeyValueService<T> KeyValues
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_namespaceId))
                {
                    throw new ArgumentException("can't use key values without providing a namespace id");
                }

                return _keyValueService;
            }
        }

        public INamespaceService Namespaces { get; }
    }
}
