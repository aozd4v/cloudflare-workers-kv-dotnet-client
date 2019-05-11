using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.Namespaces
{
    public class CreateRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
