using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models
{
    public class CloudflareError
    {
        [JsonProperty("code")]
        public long Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
