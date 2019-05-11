using System;
using CloudflareWorkersKv.Client.Models;

namespace CloudflareWorkersKv.Client.Exceptions
{
    public class NamespaceFormattingException : Exception
    {
        public NamespaceFormattingException() : base(Errors.NamespaceFormatting)
        {
        }

        public NamespaceFormattingException(string message) : base(message)
        {
        }
    }
}
