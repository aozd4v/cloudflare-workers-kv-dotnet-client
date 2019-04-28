using System;

namespace CloudflareWorkersKv.Client
{
    public class NamespaceFormattingException : Exception
    {
        public NamespaceFormattingException() : base(Errors.NamespaceFormattingErrorMessage)
        {
        }

        public NamespaceFormattingException(string message) : base(message)
        {
        }
    }
}
