using System.Collections.Generic;
using GitHub.Runner.Worker;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker
{
    public sealed class LockfileEnforcementL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_BasicEntries()
        {
            var json = "[\"actions/checkout@v4:sha1-abc123\",\"actions/setup-go@v5:sha1-def456\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Equal(2, result.Count);
            Assert.Equal("abc123", result["actions/checkout@v4"]);
            Assert.Equal("def456", result["actions/setup-go@v5"]);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_PathActions()
        {
            var json = "[\"actions/cache/save@v4:sha1-aaa\",\"actions/cache/restore@v4:sha1-bbb\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Equal(2, result.Count);
            Assert.Equal("aaa", result["actions/cache/save@v4"]);
            Assert.Equal("bbb", result["actions/cache/restore@v4"]);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_SHA256()
        {
            var json = "[\"actions/checkout@v4:sha256-abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890ab\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Single(result);
            Assert.Equal("abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890ab", result["actions/checkout@v4"]);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_LegacyGitHubComPrefix()
        {
            var json = "[\"github.com/actions/checkout@v4:sha1-abc123\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Single(result);
            Assert.Equal("abc123", result["actions/checkout@v4"]);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_EmptyAndNull()
        {
            Assert.Empty(ActionManager.ParseLockfileDependencies(""));
            Assert.Empty(ActionManager.ParseLockfileDependencies(null));
            Assert.Empty(ActionManager.ParseLockfileDependencies("[]"));
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_MalformedEntries()
        {
            // No colon separator -- should be skipped
            var json = "[\"actions/checkout@v4-no-colon\",\"actions/setup-go@v5:sha1-good\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Single(result);
            Assert.Equal("good", result["actions/setup-go@v5"]);
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ParseLockfileDependencies_CaseInsensitiveLookup()
        {
            var json = "[\"Actions/Checkout@v4:sha1-abc123\"]";
            var result = ActionManager.ParseLockfileDependencies(json);

            Assert.Equal("abc123", result["actions/checkout@v4"]);
        }
    }
}
