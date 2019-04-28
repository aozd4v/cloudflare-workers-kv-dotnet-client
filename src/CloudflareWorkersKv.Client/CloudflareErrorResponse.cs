using System.Collections.Generic;

namespace CloudflareWorkersKv.Client
{
    internal class CloudflareErrorResponse
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public IEnumerable<CloudflareError> Errors { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}
