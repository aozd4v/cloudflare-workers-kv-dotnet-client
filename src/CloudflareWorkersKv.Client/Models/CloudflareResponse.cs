using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models
{
    public class CloudflareResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("errors")]
        public IEnumerable<CloudflareError> Errors { get; set; }
        [JsonProperty("messages")]
        public IEnumerable<string> Messages { get; set; }
    }
}
