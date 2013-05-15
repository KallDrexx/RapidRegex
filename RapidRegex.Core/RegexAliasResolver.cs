using System.Linq;
using System.Text.RegularExpressions;

namespace RapidRegex.Core
{
    public class RegexAliasResolver
    {
        private readonly RegexAlias[] _aliases;

        public RegexAliasResolver(RegexAlias[] regexAliases)
        {
            _aliases = (regexAliases ?? new RegexAlias[0]).Where(x => x != null).ToArray();
            CompileDependentAliases();
        }

        public string ResolveToRegex(string aliasedPattern)
        {
            foreach (var alias in _aliases)
            {
                var replacePattern = "%{" + alias.Name + "}";
                aliasedPattern = Regex.Replace(aliasedPattern, replacePattern, alias.RegexPattern);
            }

            return aliasedPattern;
        }

        private void CompileDependentAliases()
        {
            const string aliasPattern = @"%{\w+}";

            foreach (var alias in _aliases)
            {
                var matches = Regex.Matches(alias.RegexPattern, aliasPattern);
                foreach (var match in matches.Cast<Match>())
                {
                    // Extract the name from the alias
                    var name = match.Value.Substring(2, match.Value.Length - 3);
                    
                    // Find an alias with the specified name and put its 
                    //   regex pattern into the current alias' pattern
                    var subAlias = _aliases.FirstOrDefault(x => x.Name == name);
                    if (subAlias != null)
                        alias.RegexPattern = alias.RegexPattern.Replace(match.Value, subAlias.RegexPattern);
                }
            }
        }
    }
}
