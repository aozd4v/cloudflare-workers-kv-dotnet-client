using System.Collections.Generic;

namespace CloudflareWorkersKv.Client
{
    public class ListResult
    {
        public IEnumerable<string> Keys { get; set; }
        public string Cursor { get; set; }

        public ListResult(IEnumerable<string> keys,
                          string cursor)
        {
            Keys = keys;
            Cursor = cursor;
        }
    }
}
