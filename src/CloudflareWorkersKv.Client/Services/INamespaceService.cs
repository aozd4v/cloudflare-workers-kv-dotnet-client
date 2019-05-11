using System.Collections.Generic;
using System.Threading.Tasks;
using CloudflareWorkersKv.Client.Models.Namespaces;

namespace CloudflareWorkersKv.Client.Services
{
    public interface INamespaceService
    {
        Task<string> Create(string name);
        Task Delete(string id);
        Task<IEnumerable<Namespace>> List();
        Task Rename(string id, string name);
    }
}
