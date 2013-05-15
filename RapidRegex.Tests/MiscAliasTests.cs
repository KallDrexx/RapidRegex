using System.Text.RegularExpressions;
using NUnit.Framework;
using RapidRegex.Core;
using RapidRegex.Core.Aliases;

namespace RapidRegex.Tests
{
    [TestFixture]
    public class MiscAliasTests
    {
        [Test]
        public void Has_Alias_For_Hex_Values()
        {
            var resolver = new RegexAliasResolver(new [] { Misc.Hex});
            var realPattern = resolver.ResolveToRegex("%{HEX}");

            var match1 = Regex.Match("#a3c113", realPattern);
            var match2 = Regex.Match("#4d82h4", realPattern);

            Assert.IsTrue(match1.Success, "Valid hex value was not matched");
            Assert.IsFalse(match2.Success, "Invalid hex value was incorrectly matched");
        }

        [Test]
        public void Has_Alias_For_IP_Addresses()
        {
            var resolver = new RegexAliasResolver(new[] { Misc.IpAddress });
            var realPattern = resolver.ResolveToRegex("%{IPAddress}");

            var match1 = Regex.Match("192.168.111.111", realPattern);
            var match2 = Regex.Match("555.555.555.555", realPattern);

            Assert.IsTrue(match1.Success, "Valid ip address was not matched");
            Assert.IsFalse(match2.Success, "Invalid ip address was incorrectly matched");
        }

        [Test]
        public void Has_Alias_For_Email_Addresses()
        {
            var resolver = new RegexAliasResolver(new[] { Misc.Email });
            var realPattern = resolver.ResolveToRegex("%{Email}");

            var match1 = Regex.Match("test.me@abcdefg.com", realPattern);
            var match2 = Regex.Match("abcdefg@abc", realPattern);

            Assert.IsTrue(match1.Success, "Valid email was not matched");
            Assert.IsFalse(match2.Success, "Invalid email address was incorrectly matched");
        }

        [Test]
        public void Has_Alias_For_Urls()
        {
            var resolver = new RegexAliasResolver(new[] { Misc.Url });
            var realPattern = resolver.ResolveToRegex("%{Url}");

            var match1 = Regex.Match("https://github.com/KallDrexx/RapidRegex#!test1234", realPattern);
            var match2 = Regex.Match("abcdefg/abc", realPattern);

            Assert.IsTrue(match1.Success, "Valid url was not matched");
            Assert.IsFalse(match2.Success, "Invalid url was incorrectly matched");
        }
    }
}
