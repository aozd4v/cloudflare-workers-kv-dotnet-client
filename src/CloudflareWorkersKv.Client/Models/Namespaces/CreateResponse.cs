using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.Namespaces
{
    public class CreateResponse : CloudflareResponse
    {
        [JsonProperty("result")]
        public CreateResult Result { get; set; }
    }
}
