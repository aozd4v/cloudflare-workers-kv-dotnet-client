using System;

namespace CloudflareWorkersKv.Client.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Authentication error")
        {
        }
    }
}
