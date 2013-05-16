using System.IO;
using System.Linq;
using NUnit.Framework;
using RapidRegex.Core;

namespace RapidRegex.Tests
{
    [TestFixture]
    public class BasicAliasConfigReaderTests
    {
        [Test]
        public void Can_Read_Simple_Config_Stream()
        {
            const string testInput = "Test1 [a-z]\nTest2 [0-9]";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(testInput);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var results = BasicAliasConfigReader.ParseStream(stream);

            Assert.IsNotNull(results, "Returned alias enumerable was null");
            var array = results.ToArray();

            Assert.IsNotEmpty(array, "Returned alias array is emtpy");
            Assert.AreEqual(2, array.Length, "Returned alias array has an incorrect number of elements");
            Assert.AreEqual("Test1", array[0].Name, "First alias had an incorrect name");
            Assert.AreEqual("[a-z]", array[0].RegexPattern, "First alais had an incorrect pattern");
            Assert.AreEqual("Test2", array[1].Name, "Second alias had an incorrect name");
            Assert.AreEqual("[0-9]", array[1].RegexPattern, "Second alais had an incorrect pattern");
        }

        [Test]
        public void Can_Read_Config_Stream_With_Spaces_In_Regex_Pattern()
        {
            const string testInput = "Test1 [a-z ]\nTest2 [0-9 ]";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(testInput);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var results = BasicAliasConfigReader.ParseStream(stream);

            Assert.IsNotNull(results, "Returned alias enumerable was null");
            var array = results.ToArray();

            Assert.IsNotEmpty(array, "Returned alias array is emtpy");
            Assert.AreEqual(2, array.Length, "Returned alias array has an incorrect number of elements");
            Assert.AreEqual("Test1", array[0].Name, "First alias had an incorrect name");
            Assert.AreEqual("[a-z ]", array[0].RegexPattern, "First alais had an incorrect pattern");
            Assert.AreEqual("Test2", array[1].Name, "Second alias had an incorrect name");
            Assert.AreEqual("[0-9 ]", array[1].RegexPattern, "Second alais had an incorrect pattern");
        }

        [Test]
        public void Ignores_Comments_Starting_With_Hash()
        {
            const string testInput = "#Test1 [a-z]\nTest2 [0-9]";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(testInput);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var results = BasicAliasConfigReader.ParseStream(stream);

            Assert.IsNotNull(results, "Returned alias enumerable was null");
            var array = results.ToArray();

            Assert.IsNotEmpty(array, "Returned alias array is emtpy");
            Assert.AreEqual(1, array.Length, "Returned alias array has an incorrect number of elements");
            Assert.AreEqual("Test2", array[0].Name, "Second alias had an incorrect name");
            Assert.AreEqual("[0-9]", array[0].RegexPattern, "Second alais had an incorrect pattern");
        }
    }
}
