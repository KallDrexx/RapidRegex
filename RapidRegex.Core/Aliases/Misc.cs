namespace RapidRegex.Core.Aliases
{
    public static class Misc
    {
        public static  RegexAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new RegexAlias {Name = "HEX", RegexPattern = @"#?([a-f0-9]{6}|[a-f0-9]{3})\b"}
                };
            }
        }
    }
}
