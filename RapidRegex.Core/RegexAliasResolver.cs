using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RapidRegex.Core
{
    public class RegexAliasResolver
    {
        private const string AliasPattern = @"%{\w+}";

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
            // Add this as a computed dependency to detect circular dependencies
            computedAliases.Add(alias);

            foreach (var subAliasName in GetSubAliasNames(alias))
            {
                var matchName = string.Concat("%{", subAliasName, "}");

                // Find an alias with the specified name and put its 
                //   regex pattern into the current alias' pattern
                var subAlias = _aliases.FirstOrDefault(x => x.Name == subAliasName);
                if (subAlias != null)
                {
                    // If the subAlias has already been computed this run,
                    //   we have a circular dependency
                    if (computedAliases.Contains(subAlias))
                        throw new InvalidOperationException("Circular dependency detected while computing alias dependencies");

                    // Compute the sub alias' regex
                    ComputeRawRegex(subAlias, computedAliases.ToList());
                    alias.RegexPattern = alias.RegexPattern.Replace(matchName, subAlias.RegexPattern);
                }
            }
        }

        private IEnumerable<string> GetSubAliasNames(RegexAlias alias)
        {
            return Regex.Matches(alias.RegexPattern, AliasPattern)
                        .Cast<Match>()
                        .Select(x => x.Value.Substring(2, x.Value.Length - 3))
                        .Distinct();
        }
    }
}
