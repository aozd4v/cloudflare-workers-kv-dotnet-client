namespace CloudflareWorkersKv.Client
{
    internal static class Errors
    {
        public const string NamespaceFormattingErrorMessage = "could not parse UUID from request's namespace_id";
        public const string JsonDeserializationErrorMessage = "Response could not be deserialized to JSON";
    }
}
