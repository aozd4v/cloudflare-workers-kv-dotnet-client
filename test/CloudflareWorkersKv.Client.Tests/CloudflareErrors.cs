namespace CloudflareWorkersKv.Client.Tests
{
    public static class CloudflareErrors
    {
        public const string AuthenticationError = @"
            {
              ""success"": false,
              ""errors"": [
                {
                  ""code"": 10000,
                  ""message"": ""Authentication error""
                }
              ]
            }";

        public const string KeyNotFoundError = @"
            {
              ""result"": null,
              ""success"": false,
              ""errors"": [
                {
                  ""code"": 10009,
                  ""message"": ""key not found""
                }
              ],
              ""messages"": []
            }";

        public const string NamespaceFormattingError = @"
            {
              ""result"": null,
              ""success"": false,
              ""errors"": [
                {
                  ""code"": 10011,
                  ""message"": ""could not parse UUID from request's namespace_id: 'uuid: incorrect UUID length: fe95b5f47aa040aca4cec21244n1ta0'""
                }
              ],
              ""messages"": []
            }";

        public const string NamespaceNotFound = @"
            {
              ""result"": null,
              ""success"": false,
              ""errors"": [
                {
                  ""code"": 10013,
                  ""message"": ""namespace not found""
                }
              ],
              ""messages"": []
            }";
    }
}
