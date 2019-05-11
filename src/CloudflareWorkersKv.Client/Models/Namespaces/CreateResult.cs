using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.Namespaces
{
    public class CreateResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
