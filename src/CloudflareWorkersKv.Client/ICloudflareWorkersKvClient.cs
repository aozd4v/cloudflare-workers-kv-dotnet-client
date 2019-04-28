using System.Threading.Tasks;

namespace CloudflareWorkersKv.Client
{
    public interface ICloudflareWorkersKvClient
    {
        Task<T> Read<T>(string key);
        Task Write<T>(string key, T value);
    }
}
