using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Models;
using CloudflareWorkersKv.Client.Models.KeyValues;

namespace CloudflareWorkersKv.Client.Services
{
    public interface IKeyValueService<T>
    {
        Task Delete(string key);
        Task<T> Read(string key);
        Task<ListResult> List(string cursor = null);
        Task Write(string key, T value);
    }
}
