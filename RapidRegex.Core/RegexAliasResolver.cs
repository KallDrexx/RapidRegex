using System;
using System.Collections.Generic;
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
            foreach (var alias in _aliases)
                ComputeRawRegex(alias, new List<RegexAlias>());
        }

        private void ComputeRawRegex(RegexAlias alias, List<RegexAlias> computedAliases)
        {
            const string aliasPattern = @"%{\w+}";

            // Add this as a computed dependency to detect circular dependencies
            computedAliases.Add(alias);

            var matches = Regex.Matches(alias.RegexPattern, aliasPattern);
            foreach (var match in matches.Cast<Match>())
            {
                // Extract the name from the alias
                var name = match.Value.Substring(2, match.Value.Length - 3);

                // Find an alias with the specified name and put its 
                //   regex pattern into the current alias' pattern
                var subAlias = _aliases.FirstOrDefault(x => x.Name == name);
                if (subAlias != null)
                {
                    // If the subAlias has already been computed this run,
                    //   we have a circular dependency
                    if (computedAliases.Contains(subAlias))
                        throw new InvalidOperationException("Circular dependency detected while computing alias dependencies");

                    // Compute the sub alias' regex
                    ComputeRawRegex(subAlias, computedAliases);
                    alias.RegexPattern = alias.RegexPattern.Replace(match.Value, subAlias.RegexPattern);
                }
            }
        }
    }
}
