using System;
using Xunit;

namespace CloudflareWorkersKv.Client.Tests
{
    public class GivenCloudflareWorkersKvClient
    {
        [Fact]
        public void WhenConstructingWithEmptyEmail_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CloudflareWorkersKvClient<object>(null, "key", "account"));
        }

        [Fact]
        public void WhenConstructingWithEmptyKey_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CloudflareWorkersKvClient<object>("email", null, "account"));
        }

        [Fact]
        public void WhenConstructingWithEmptyAccount_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new CloudflareWorkersKvClient<object>("email", "key", null));
        }

        [Fact]
        public void WhenConstructingWithEmptyNamespace_ButAttemptToGetTheKeyValueService_ShouldThrowArgumentException()
        {
            var client = new CloudflareWorkersKvClient<object>("email", "key", "account");

            Assert.Throws<ArgumentException>(() => client.KeyValues);
        }
    }
}
