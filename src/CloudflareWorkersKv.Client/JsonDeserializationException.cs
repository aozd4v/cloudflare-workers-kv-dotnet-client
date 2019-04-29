using System;

namespace CloudflareWorkersKv.Client
{
    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException(Exception ex) : base(Errors.JsonDeserialization, ex)
        {
        }
    }
}
