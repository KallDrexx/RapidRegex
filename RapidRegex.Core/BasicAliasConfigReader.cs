using System.Collections.Generic;
using System.IO;

namespace RapidRegex.Core
{
    public static class BasicAliasConfigReader
    {
        public static IEnumerable<RegexAlias> ParseStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;

                    var spaceIndex = line.IndexOf(' ');
                    if (spaceIndex <= 0)
                        continue; // Invalid line

                    if (line.StartsWith("#"))
                        continue;

                    var name = line.Substring(0, spaceIndex);
                    var pattern = line.Substring(spaceIndex + 1);

                    yield return new RegexAlias
                    {
                        Name = name,
                        RegexPattern = pattern
                    };
                }
            }
        }

        public static IEnumerable<RegexAlias> ParseConfigFile(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
                return ParseStream(stream);
        }
    }
}
