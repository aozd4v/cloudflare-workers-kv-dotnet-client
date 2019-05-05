using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client
{
    public class CloudflareWorkersKvClient<T> : ICloudflareWorkersKvClient<T>
    {
        private readonly object _headers;
        private readonly string _baseUrl = "https://api.cloudflare.com/client/v4/accounts";
        private readonly string _namespacesUrl;
        private readonly string _keyUrl;

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
        }

        public async Task Delete(string key)
        {
            var url = GetKeyUrl(key);

            try
            {
                await url
                    .WithHeaders(_headers)
                    .DeleteAsync();
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }
        }

        public async Task<T> Read(string key)
        {
            var url = GetKeyUrl(key);
            var responseObject = default(T);

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
            var url = $"{_namespacesUrl}/keys";

            if (!string.IsNullOrWhiteSpace(cursor))
            {
                url = $"{url}?cursor={cursor}";
            }

            try
            {
                var response = await url
                    .WithHeaders(_headers)
                    .GetJsonAsync<ListKeysResponse>();

                var keys = new List<string>();

                foreach (var result in response.Result)
                {
                    keys.Add(result.Name);
                }

                return new ListResult(keys, response.ResultInfo.Cursor);
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }

            return null;
        }

        public async Task Write(string key, T value)
        {
            var url = GetKeyUrl(key);

            try
            {
                await url
                    .WithHeaders(_headers)
                    .PutJsonAsync(value);
            }
            catch (FlurlHttpException ex)
            {
                await HttpExceptionHandling(ex);
            }
        }

        private string GetKeyUrl(string key)
        {
            return $"{_keyUrl}/{key}";
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
