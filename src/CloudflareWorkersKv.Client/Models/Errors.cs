namespace CloudflareWorkersKv.Client.Models
{
    internal static class Errors
    {
        internal const string NamespaceFormatting = "could not parse UUID from request's namespace_id";
        internal const string NamespaceNotFound = "namespace not found";
        internal const string NamespaceAlreadyExists = "a namespace with this account ID and title already exists";
        internal const string JsonDeserialization = "Response could not be deserialized to JSON";
    }
}
