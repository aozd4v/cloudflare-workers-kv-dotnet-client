using System;
using CloudflareWorkersKv.Client.Models;

namespace CloudflareWorkersKv.Client.Exceptions
{
    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException(Exception ex) : base(Errors.JsonDeserialization, ex)
        {
        }
    }
}
