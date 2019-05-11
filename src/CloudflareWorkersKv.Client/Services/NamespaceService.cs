using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Extensions;
using CloudflareWorkersKv.Client.Models.KeyValues;
using CloudflareWorkersKv.Client.Models.Namespaces;
using Flurl.Http;

namespace CloudflareWorkersKv.Client.Services
{
    public class NamespaceService : INamespaceService
    {
        private readonly string _url;
        private readonly object _headers;

        public NamespaceService(string url, object headers)
        {
            _url = url;
            _headers = headers;
        }

        public async Task<string> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name cannot be null or empty");
            }

            try
            {
                var request = new CreateRequest
                {
                    Title = name
                };

                var response = await _url
                    .WithHeaders(_headers)
                    .PostJsonAsync(request)
                    .ReceiveJson<CreateResponse>();

                return response.Result.Id;
            }
            catch (FlurlHttpException ex)
            {
                await ex.ThrowContextualException();
            }

            return null;
        }

        public async Task Delete(string id)
        {
            var url = $"{_url}/{id}";

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

        public async Task<IEnumerable<Namespace>> List()
        {
            try
            {
                var response = await _url
                    .WithHeaders(_headers)
                    .GetJsonAsync<ListNamespacesResponse>();

                return response.Result;
            }
            catch (FlurlHttpException ex)
            {
                await ex.ThrowContextualException();
            }

            return null;
        }

        public async Task Rename(string id, string name)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name cannot be null or empty");
            }

            var url = $"{_url}/{id}";

            try
            {
                var request = new UpdateRequest
                {
                    Title = name
                };

                await url
                    .WithHeaders(_headers)
                    .PutJsonAsync(request);
            }
            catch (FlurlHttpException ex)
            {
                await ex.ThrowContextualException();
            }
        }
    }
}
