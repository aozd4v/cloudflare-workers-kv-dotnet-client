Cloudflare Workers KV Client for .NET
===========================

![build status](https://travis-ci.org/aozd4v/cloudflare-workers-kv-dotnet-client.svg?branch=master)

.NET platform compatibility
---------------------------

This version is compatible with .NET Standard 1.3-2.0.

Quick setup
-----------

1. Use [NuGet](http://docs.nuget.org/docs/start-here/using-the-package-manager-console) to add the .NET SDK to your project:

        Install-Package CloudflareWorkersKv.Client

2. Import the package:

        using CloudflareWorkersKv.Client;

3. Create a new client with your credentials:

        ICloudflareWorkersKvClient<MyType> kvClient = new CloudflareWorkersKvClient<MyType>("email", "authKey", "accountId", "namespaceId");