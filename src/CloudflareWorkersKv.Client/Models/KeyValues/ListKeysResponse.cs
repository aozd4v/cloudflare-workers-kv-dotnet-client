using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client.Models.KeyValues
{
    internal class ListKeysResponse : CloudflareResponse
    {
        [JsonProperty("result")]
        public IEnumerable<ListKeysResult> Result { get; set; }
        [JsonProperty("result_info")]
        public ResultInfo ResultInfo { get; set; }
    }
}
