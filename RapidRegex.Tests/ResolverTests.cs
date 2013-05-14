using NUnit.Framework;
using RapidRegex.Core;

namespace RapidRegex.Tests
{
    [TestFixture]
    public class ResolverTests
    {
        [Test]
        public void Can_Convert_Basic_Alias()
        {
            const string inputPattern = "testing%{numbers}";
            const string expectedResult = "testing[0-9]+";

            var alias = new RegexAlias
            {
                Name = "numbers",
                RegexPattern = @"[0-9]+"
            };

            var resolver = new RegexAliasResolver(new[] {alias});
            var pattern = resolver.ConvertToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Can_Convert_Multiple_Aliases()
        {
            const string inputPattern = "%{letters}%{numbers}";
            const string expectedResult = "[a-z]+[0-9]+";

            var alias = new RegexAlias
            {
                Name = "numbers",
                RegexPattern = @"[0-9]+"
            };

            var alias2 = new RegexAlias
            {
                Name = "letters",
                RegexPattern = @"[a-z]+"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2 });
            var pattern = resolver.ConvertToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }
    }
}
