using System.Collections.Generic;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Extensions;
using CloudflareWorkersKv.Client.Models;
using CloudflareWorkersKv.Client.Models.KeyValues;
using Flurl.Http;

namespace CloudflareWorkersKv.Client.Services
{
    public class KeyValueService<T> : IKeyValueService<T>
    {
        private readonly string _url;
        private readonly object _headers;

        public KeyValueService(string url,
                               object headers)
        {
            _url = url;
            _headers = headers;
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
                await ex.ThrowContextualException();
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
                await ex.ThrowContextualException();
            }

            return responseObject;
        }

        public async Task<ListResult> List(string cursor = null)
        {
            var url = $"{_url}/keys";

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
                await ex.ThrowContextualException();
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
                await ex.ThrowContextualException();
            }
        }

        private string GetKeyUrl(string key)
        {
            return $"{_url}/values/{key}";
        }
    }
}
