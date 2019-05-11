using CloudflareWorkersKv.Client.Services;

namespace CloudflareWorkersKv.Client
{
    public interface ICloudflareWorkersKvClient<T>
    {
        IKeyValueService<T> KeyValues { get; }
        INamespaceService Namespaces { get; }
    }
}
