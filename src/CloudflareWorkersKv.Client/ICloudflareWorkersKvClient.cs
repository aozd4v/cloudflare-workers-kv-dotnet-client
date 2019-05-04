using System.Threading.Tasks;

namespace CloudflareWorkersKv.Client
{
    public interface ICloudflareWorkersKvClient<T>
    {
        Task Delete(string key);
        Task<T> Read(string key);
        Task<ListResult> List(string cursor = null);
        Task Write(string key, T value);
    }
}
