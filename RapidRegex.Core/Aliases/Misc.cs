namespace RapidRegex.Core.Aliases
{
    public static class Misc
    {
        public static RegexAlias[] AllAliases
        {
            get
            {
                return new[]
                {
                    Hex, IpAddress, Email, Url
                };
            }
        }

        public static readonly RegexAlias Hex = new RegexAlias
        {
            Name = "HEX",
            RegexPattern = @"\b#?([a-f0-9]{6}|[a-f0-9]{3})\b"
        };

        public static readonly RegexAlias IpAddress = new RegexAlias
        {
            Name = "IPAddress",
            RegexPattern =
                @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
        };

        public static readonly RegexAlias Email = new RegexAlias
        {
            Name = "Email",
            RegexPattern = @"([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})"
        };

        public static readonly RegexAlias Url = new RegexAlias
        {
            Name = "Url",
            RegexPattern = @"(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?"
        };
    }
}
