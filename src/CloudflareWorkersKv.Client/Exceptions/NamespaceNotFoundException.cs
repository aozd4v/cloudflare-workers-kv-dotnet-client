using System;
using CloudflareWorkersKv.Client.Models;

namespace CloudflareWorkersKv.Client.Exceptions
{
    public class NamespaceNotFoundException : Exception
    {
        public NamespaceNotFoundException() : base(Errors.NamespaceNotFound)
        {
        }
    }
}
