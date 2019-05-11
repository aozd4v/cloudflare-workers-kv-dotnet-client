using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.Namespaces
{
    public class UpdateRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
