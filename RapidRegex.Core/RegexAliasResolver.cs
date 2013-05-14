using System.Text.RegularExpressions;

namespace RapidRegex.Core
{
    public class RegexAliasResolver
    {
        private readonly RegexAlias[] _aliases;

        public RegexAliasResolver(RegexAlias[] regexAliases)
        {
            _aliases = regexAliases;
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
    }
}
