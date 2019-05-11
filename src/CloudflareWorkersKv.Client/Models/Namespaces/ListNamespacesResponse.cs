using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.Namespaces
{
    internal class ListNamespacesResponse : CloudflareResponse
    {
        [JsonProperty("result")]
        internal IEnumerable<Namespace> Result { get; set; }
        [JsonProperty("result_info")]
        internal ResultInfo ResultInfo { get; set; }
    }
}
