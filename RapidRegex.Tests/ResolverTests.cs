using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RapidRegex.Core;

namespace RapidRegex.Tests
{
    [TestFixture]
    public class ResolverTests
    {
        [Test]
        public void Can_Resolve_Basic_Alias()
        {
            const string inputPattern = "testing%{numbers}";
            const string expectedResult = "testing[0-9]+";

            var alias = new RegexAlias
            {
                Name = "numbers",
                RegexPattern = @"[0-9]+"
            };

            var resolver = new RegexAliasResolver(new[] {alias});
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Can_Resolve_Multiple_Aliases()
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
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Returns_Input_When_No_Aliases_Provided()
        {
            const string testPattern = "%{numbers}";

            var resolver = new RegexAliasResolver(new RegexAlias[0]);
            var result = resolver.ResolveToRegex(testPattern);

            Assert.AreEqual(testPattern, result, "Returned result was not correct");
        }

        [Test]
        public void Returns_Input_When_Null_Aliase_Array_Provided()
        {
            const string testPattern = "%{numbers}";

            var resolver = new RegexAliasResolver(null);
            var result = resolver.ResolveToRegex(testPattern);

            Assert.AreEqual(testPattern, result, "Returned result was not correct");
        }

        [Test]
        public void Null_Aliases_Are_Ignored()
        {
            const string inputPattern = "%{letters}%{numbers}";
            const string expectedResult = "%{letters}[0-9]+";

            var alias = new RegexAlias
            {
                Name = "numbers",
                RegexPattern = @"[0-9]+"
            };

            var resolver = new RegexAliasResolver(new[] { alias, null });
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Can_Resolve_Dependent_Aliases()
        {
            const string inputPattern = "%{characters}";
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

            var alias3 = new RegexAlias
            {
                Name = "characters",
                RegexPattern = "%{letters}%{numbers}"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2, alias3 });
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Can_Resolve_Deep_Dependent_Aliases()
        {
            const string inputPattern = "%{test2}";
            const string expectedResult = "abcdefg";

            var alias = new RegexAlias
            {
                Name = "test1",
                RegexPattern = @"%{test3}"
            };

            var alias2 = new RegexAlias
            {
                Name = "test2",
                RegexPattern = @"%{test1}%{test4}"
            };

            var alias3 = new RegexAlias
            {
                Name = "test3",
                RegexPattern = "abcd"
            };

            var alias4 = new RegexAlias
            {
                Name = "test4",
                RegexPattern = "efg"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2, alias3, alias4 });
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Exception_Thrown_When_Circular_Alias_Dependency_Exists()
        {
            var alias = new RegexAlias
            {
                Name = "test1",
                RegexPattern = @"%{test2}"
            };

            var alias2 = new RegexAlias
            {
                Name = "test2",
                RegexPattern = @"%{test3}"
            };

            var alias3 = new RegexAlias
            {
                Name = "test3",
                RegexPattern = @"%{test1}"
            };

            new RegexAliasResolver(new[] {alias, alias2, alias3});
        }

        [Test]
        public void Can_Resolve_Dependent_Aliases_With_Multiple_References_To_The_Same_Alias()
        {
            const string inputPattern = "%{test1}";
            const string expectedResult = "abcabc";

            var alias = new RegexAlias
            {
                Name = "test1",
                RegexPattern = @"%{test2}%{test2}"
            };

            var alias2 = new RegexAlias
            {
                Name = "test2",
                RegexPattern = @"abc"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2 });
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void Can_Resolve_Dependent_Aliases_With_Diamond_Dependencies()
        {
            const string inputPattern = "%{test1}";
            const string expectedResult = "abcabc";

            var alias = new RegexAlias
            {
                Name = "test1",
                RegexPattern = @"%{test2}%{test3}"
            };

            var alias2 = new RegexAlias
            {
                Name = "test2",
                RegexPattern = @"%{test4}"
            };

            var alias3 = new RegexAlias
            {
                Name = "test3",
                RegexPattern = @"%{test4}"
            };

            var alias4 = new RegexAlias
            {
                Name = "test4",
                RegexPattern = @"abc"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2, alias3, alias4 });
            var pattern = resolver.ResolveToRegex(inputPattern);

            Assert.AreEqual(expectedResult, pattern, "Returned pattern was not correct");
        }

        [Test]
        public void IP_Address_Test()
        {
            var alias = new RegexAlias
            {
                Name = "IPAddress",
                RegexPattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
            };

            var resolver = new RegexAliasResolver(new[] { alias });

            const string pattern = "connection from %{IPAddress}";
            const string test1 = "connection from 192.168.0.1";
            const string test2 = "connection from 555.555.555.555";
            
            // Resolve the pattern into becomes "connection from \b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
            var regexPattern = resolver.ResolveToRegex(pattern);

            // Run the regex
            var match1 = Regex.Match(test1, regexPattern);
            var match2 = Regex.Match(test2, regexPattern);

            Assert.IsTrue(match1.Success);
            Assert.IsFalse(match2.Success);
        }

        [Test]
        public void Chained_IP_Address_Test1()
        {
            var alias = new RegexAlias
            {
                Name = "IPDigit",
                RegexPattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"
            };

            var alias2 = new RegexAlias
            {
                Name = "IPAddress",
                RegexPattern = @"%{IPDigit}\.%{IPDigit}\.%{IPDigit}\.%{IPDigit}"
            };

            var resolver = new RegexAliasResolver(new[] { alias, alias2 });

            const string pattern = "connection from %{IPAddress}";
            const string test1 = "connection from 192.168.0.1";
            const string test2 = "connection from 555.555.555.555";

            // Resolve the pattern into becomes "connection from \b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
            var regexPattern = resolver.ResolveToRegex(pattern);

            // Run the regex
            var match1 = Regex.Match(test1, regexPattern);
            var match2 = Regex.Match(test2, regexPattern);

            Assert.IsTrue(match1.Success);
            Assert.IsFalse(match2.Success);
        }
    }
}
