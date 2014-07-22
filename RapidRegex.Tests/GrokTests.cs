using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RapidRegex.Core;

namespace RapidRegex.Tests
{
    [TestFixture]
    public class GrokTests
    {
        [Test]
        public void TestGrokTimes()
        {
            const string inputPattern = "%{HOST:host} %{Email:email} %{QUOTEDSTRING:qs}";           
            const string test1 = "DEVEFONTANA.test.com foo.bar@gmail.com 'now what is it?'";
      
            var resolver = new RegexGrokResolver();
            var pattern = resolver.ResolveToRegex(inputPattern);
            var match1 = Regex.Match(test1, pattern);

            Assert.IsTrue(match1.Success);

            if (match1.Success)
            {
                var regex = new Regex(pattern);
                var namedCaptures = regex.MatchNamedCaptures(test1);
                Assert.AreEqual(namedCaptures["host"], "DEVEFONTANA.test.com");
                Assert.AreEqual(namedCaptures["email"], "foo.bar@gmail.com");
                Assert.AreEqual(namedCaptures["qs"], "'now what is it?'");
            }
        }

        [Test]
        public void TestSyslog()
        {
            const string inputPattern = "%{SYSLOGLINE}";
            const string test1 = "Nov 21 17:27:53 HANNIBAL MyProgram[13163]: Program started by User 1000";
            var resolver = new RegexGrokResolver();
            var pattern = resolver.ResolveToRegex(inputPattern);
            var match1 = Regex.Match(test1, pattern);

            Assert.IsTrue(match1.Success);

            if (match1.Success)
            {
                var regex = new Regex(pattern);
                var namedCaptures = regex.MatchNamedCaptures(test1);
                Assert.AreEqual(namedCaptures["timestamp"], "Nov 21 17:27:53");
                Assert.AreEqual(namedCaptures["logsource"], "HANNIBAL");
                Assert.AreEqual(namedCaptures["program"], "MyProgram");
                Assert.AreEqual(namedCaptures["pid"], "13163");
                Assert.AreEqual(namedCaptures["message"], "Program started by User 1000");       
            }
        }        
    }
}
