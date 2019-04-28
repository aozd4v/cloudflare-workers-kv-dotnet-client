using System;

namespace CloudflareWorkersKv.Client
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Authentication error")
        {
        }
    }
}
