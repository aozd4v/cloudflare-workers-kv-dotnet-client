using System;
using CloudflareWorkersKv.Client.Models;

namespace CloudflareWorkersKv.Client.Exceptions
{
    public class NamespaceAlreadyExistsException : Exception
    {
        public NamespaceAlreadyExistsException(Exception exception) : base(Errors.NamespaceAlreadyExists, exception)
        {
        }
    }
}
