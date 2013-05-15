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
            var aliases = Misc.Aliases;
            var resolver = new RegexAliasResolver(aliases);
            var realPattern = resolver.ResolveToRegex("%{HEX}");

            var match1 = Regex.Match("#a3c113", realPattern);
            var match2 = Regex.Match("#4d82h4", realPattern);

            Assert.IsTrue(match1.Success, "Valid hex value was not matched");
            Assert.IsFalse(match2.Success, "Invalid hex value was incorrectly matched");
        }
    }
}
