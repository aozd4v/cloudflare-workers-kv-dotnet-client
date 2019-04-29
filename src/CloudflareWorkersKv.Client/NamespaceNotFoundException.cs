using System;

namespace CloudflareWorkersKv.Client
{
    public class NamespaceNotFoundException : Exception
    {
        public NamespaceNotFoundException() : base(Errors.NamespaceNotFound)
        {
        }
    }
}
