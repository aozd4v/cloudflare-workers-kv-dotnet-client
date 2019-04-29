using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client
{
    public class CloudflareWorkersKvClient : ICloudflareWorkersKvClient
    {
        private readonly object _headers;
        private readonly string _baseUrl = "https://api.cloudflare.com/client/v4/accounts";
        private readonly string _namespacesUrl;
        private readonly string _keyUrl;
        private readonly string _email;
        private readonly string _authKey;
        private readonly string _namespaceId;

        public CloudflareWorkersKvClient(string email,
                                         string authKey,
                                         string accountId,
                                         string namespaceId)
        {
            _headers = new
            {
                X_Auth_Email = email,
                X_Auth_Key = authKey,
                Content_Type = "application/json"
            };

            _baseUrl = $"{_baseUrl}/{accountId}";
            _namespacesUrl = $"{_baseUrl}/storage/kv/namespaces/{namespaceId}";
            _keyUrl = $"{_namespacesUrl}/values";
            _email = email;
            _authKey = authKey;
            _namespaceId = namespaceId;
        }

        public async Task<T> Read<T>(string key)
        {
            var url = $"{_keyUrl}/{key}";
            T responseObject = default(T);

            try
            {
                responseObject = await url
                    .WithHeaders(_headers)
                    .GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }

            return responseObject;
        }

        public async Task<ListResult> List(string cursor = null)
        {
            var url = $"{_namespacesUrl}/list";

            if (!string.IsNullOrWhiteSpace(cursor))
            {
                url = $"{url}?cursor={cursor}";
            }

            try
            {
                var response = await url
                    .WithHeaders(_headers)
                    .GetJsonAsync<ListKeysResponse>();

                return new ListResult(response.Result.Select(x => x.Name),
                    response.ResultInfo.Cursor);
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }

            return null;
        }

        public async Task Write<T>(string key, T value)
        {
            var url = $"{_keyUrl}/{key}";

            try
            {
                var payload = JsonConvert.SerializeObject(value);

                await url
                    .WithHeaders(_headers)
                    .PutJsonAsync(payload);
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }
        }

        private async Task HttpExceptionHandling(FlurlHttpException exception)
        {
            var deserializationError = exception.Message.Contains(Errors.JsonDeserialization);

            if (deserializationError)
            {
                throw new JsonDeserializationException(exception.InnerException);
            }

            var response = await exception.Call.Response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<CloudflareErrorResponse>(response);
            var authenticationError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10000);

            if (authenticationError != null)
            {
                throw new UnauthorizedException();
            }

            var namespaceFormattingError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10011);

            if (namespaceFormattingError != null)
            {
                throw new NamespaceFormattingException(namespaceFormattingError.Message);
            }

            var namespaceNotFoundError = errorResponse.Errors.FirstOrDefault(x => x.Code == 10013);

            if (namespaceNotFoundError != null)
            {
                throw new NamespaceNotFoundException();
            }
        }
    }
}
