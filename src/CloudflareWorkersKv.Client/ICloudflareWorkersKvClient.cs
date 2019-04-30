using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudflareWorkersKv.Client
{
    public interface ICloudflareWorkersKvClient
    {
        Task Delete(string key);
        Task<T> Read<T>(string key);
        Task<ListResult> List(string cursor = null);
        Task Write<T>(string key, T value);
    }
}
