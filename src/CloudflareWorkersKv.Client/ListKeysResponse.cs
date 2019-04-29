using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudflareWorkersKv.Client
{
    internal class ListKeysResponse
    {
        [JsonProperty("result")]
        public IEnumerable<ListKeysResult> Result { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errors")]
        public IEnumerable<CloudflareError> Errors { get; set; }

        [JsonProperty("messages")]
        public IEnumerable<string> Messages { get; set; }

        [JsonProperty("result_info")]
        public ListKeysResultInfo ResultInfo { get; set; }
    }
}
