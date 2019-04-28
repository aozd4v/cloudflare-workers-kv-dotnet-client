Cloudflare Workers KV Client for .NET
===========================

.NET platform compatibility
---------------------------

This version is compatible with .NET Standard 1.4-2.0.

Quick setup
-----------

1. Use [NuGet](http://docs.nuget.org/docs/start-here/using-the-package-manager-console) to add the .NET SDK to your project:

        Install-Package CloudflareWorkersKv.Client

2. Import the package:

        using CloudflareWorkersKv.Client;

3. Create a new client with your credentials:

        ICloudflareWorkersKvClient kvClient = new CloudflareWorkersKvClient("email", "authKey", "accountId", "namespaceId");