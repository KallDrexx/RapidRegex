namespace RapidRegex.Core.Aliases
{
    public static class BaseAliases
    {
        public static RegexAlias[] All
        {
            get
            {
                return new[]
                {
                    new RegexAlias
                    {
                        Name = "HEX",
                        RegexPattern = @"\b#?([a-f0-9]{6}|[a-f0-9]{3})\b"
                    },

                    new RegexAlias
                    {
                        Name = "Email",
                        RegexPattern = @"([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})"
                    },

                    // All below taken from https://github.com/jordansissel/grok/blob/master/patterns/base
                    //  Copyright (c) 2007, Jordan Sissel.
                    new RegexAlias {Name = "USERNAME", RegexPattern = @"[a-zA-Z0-9_-]+"},
                    new RegexAlias {Name = "USER", RegexPattern = @"%{USERNAME}"},
                    new RegexAlias {Name = "INT", RegexPattern = @"(?:[+-]?(?:[0-9]+))"},
                    new RegexAlias
                    {
                        Name = "BASE10NUM",
                        RegexPattern = @"(?<![0-9.+-])(?>[+-]?(?:(?:[0-9]+(?:\.[0-9]+)?)|(?:\.[0-9]+)))"
                    },
                    new RegexAlias {Name = "NUMBER", RegexPattern = @"(?:%{BASE10NUM})"},
                    new RegexAlias
                    {
                        Name = "BASE16NUM",
                        RegexPattern = @"(?<![0-9A-Fa-f])(?:[+-]?(?:0x)?(?:[0-9A-Fa-f]+))"
                    },
                    new RegexAlias
                    {
                        Name = "BASE16FLOAT",
                        RegexPattern =
                            @"\b(?<![0-9A-Fa-f.])(?:[+-]?(?:0x)?(?:(?:[0-9A-Fa-f]+(?:\.[0-9A-Fa-f]*)?)|(?:\.[0-9A-Fa-f]+)))\b"
                    },

                    new RegexAlias {Name = "POSINT", RegexPattern = @"\b(?:[0-9]+)\b"},
                    new RegexAlias {Name = "WORD", RegexPattern = @"\b\w+\b"},
                    new RegexAlias {Name = "NOTSPACE", RegexPattern = @"\S+"},
                    new RegexAlias {Name = "SPACE", RegexPattern = @"\s*"},
                    new RegexAlias {Name = "DATA", RegexPattern = @".*?"},
                    new RegexAlias {Name = "GREEDYDATA", RegexPattern = @".*"},
                    new RegexAlias
                    {
                        Name = "QUOTEDSTRING",
                        RegexPattern =
                            @"(?:(?<!\\)(?:""(?:\\.|[^\\""])*""|(?:'(?:\\.|[^\\'])*')|(?:`(?:\\.|[^\\`])*`)))"
                    },

                    // Networking
                    new RegexAlias {Name = "MAC", RegexPattern = @"(?:%{CISCOMAC}|%{WINDOWSMAC}|%{COMMONMAC})"},
                    new RegexAlias {Name = "CISCOMAC", RegexPattern = @"(?:(?:[A-Fa-f0-9]{4}\.){2}[A-Fa-f0-9]{4})"},
                    new RegexAlias {Name = "WINDOWSMAC", RegexPattern = @"(?:(?:[A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2})"},
                    new RegexAlias {Name = "COMMONMAC", RegexPattern = @"(?:(?:[A-Fa-f0-9]{2}:){5}[A-Fa-f0-9]{2})"},
                    new RegexAlias
                    {
                        Name = "IP",
                        RegexPattern =
                            @"(?<![0-9])(?:(?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2}))(?![0-9])"
                    },
                    new RegexAlias
                    {
                        Name = "HOSTNAME",
                        RegexPattern =
                            @"\b(?:[0-9A-Za-z][0-9A-Za-z-]{0,62})(?:\.(?:[0-9A-Za-z][0-9A-Za-z-]{0,62}))*(\.?|\b)"
                    },
                    new RegexAlias {Name = "HOST", RegexPattern = @"%{HOSTNAME}"},
                    new RegexAlias {Name = "IPORHOST", RegexPattern = @"(?:%{HOSTNAME}|%{IP})"},
                    new RegexAlias {Name = "HOSTPORT", RegexPattern = @"(?:%{IPORHOST=~/\./}:%{POSINT})"},

                    // Paths
                    new RegexAlias {Name = "PATH", RegexPattern = @"(?:%{UNIXPATH}|%{WINPATH})"},
                    new RegexAlias {Name = "UNIXPATH", RegexPattern = @"(?<![\w\\/])(?:/(?:[\w_%!$@:.,-]+|\\.)*)+"},
                    new RegexAlias {Name = "#UNIXPATH", RegexPattern = @"(?<![\w\/])(?:/[^\/\s?*]*)+"},
                    new RegexAlias {Name = "LINUXTTY", RegexPattern = @"(?:/dev/pts/%{POSINT})"},
                    new RegexAlias {Name = "BSDTTY", RegexPattern = @"(?:/dev/tty[pq][a-z0-9])"},
                    new RegexAlias {Name = "TTY", RegexPattern = @"(?:%{BSDTTY}|%{LINUXTTY})"},
                    new RegexAlias {Name = "WINPATH", RegexPattern = @"(?:[A-Za-z]+:|\\)(?:\\[^\\?*]*)+"},
                    new RegexAlias {Name = "URIPROTO", RegexPattern = @"[A-Za-z]+(\+[A-Za-z+]+)?"},
                    new RegexAlias {Name = "URIHOST", RegexPattern = @"%{IPORHOST}(?::%{POSINT:port})?"},
                    new RegexAlias {Name = "URIPATH", RegexPattern = @"(?:/[A-Za-z0-9$.+!*'(),~:#%_-]*)+"},
                    new RegexAlias {Name = "URIPARAM", RegexPattern = @"\?[A-Za-z0-9$.+!*'(),~#%&/=:;_-]*"},
                    new RegexAlias {Name = "URIPATHPARAM", RegexPattern = @"%{URIPATH}(?:%{URIPARAM})?"},
                    new RegexAlias
                    {
                        Name = "URI",
                        RegexPattern = @"%{URIPROTO}://(?:%{USER}(?::[^@]*)?@)?(?:%{URIHOST})?(?:%{URIPATHPARAM})?"
                    },

                    // Months
                    new RegexAlias
                    {
                        Name = "MONTH",
                        RegexPattern =
                            @"\b(?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\b"
                    },
                    new RegexAlias {Name = "MONTHNUM", RegexPattern = @"(?:0?[1-9]|1[0-2])"},
                    new RegexAlias {Name = "MONTHDAY", RegexPattern = @"(?:3[01]|[1-2]?[0-9]|0?[1-9])"},

                    // Days
                    new RegexAlias
                    {
                        Name = "DAY",
                        RegexPattern =
                            @"(?:Mon(?:day)?|Tue(?:sday)?|Wed(?:nesday)?|Thu(?:rsday)?|Fri(?:day)?|Sat(?:urday)?|Sun(?:day)?)"
                    },

                    // Years
                    new RegexAlias {Name = "YEAR", RegexPattern = @"[0-9]+"},

                    // Time
                    new RegexAlias {Name = "HOUR", RegexPattern = @"(?:2[0123]|[01][0-9])"},
                    new RegexAlias {Name = "MINUTE", RegexPattern = @"(?:[0-5][0-9])"},
                    new RegexAlias {Name = "SECOND", RegexPattern = @"(?:(?:[0-5][0-9]|60)(?:[.,][0-9]+)?)"},
                    new RegexAlias {Name = "TIME", RegexPattern = @"(?!<[0-9])%{HOUR}:%{MINUTE}(?::%{SECOND})(?![0-9])"}
                    ,

                    // Dates
                    new RegexAlias {Name = "DATE_US", RegexPattern = @"%{MONTHNUM}[/-]%{MONTHDAY}[/-]%{YEAR}"},
                    new RegexAlias {Name = "DATE_EU", RegexPattern = @"%{YEAR}[/-]%{MONTHNUM}[/-]%{MONTHDAY}"},
                    new RegexAlias {Name = "ISO8601_TIMEZONE", RegexPattern = @"(?:Z|[+-]%{HOUR}(?::?%{MINUTE}))"},
                    new RegexAlias {Name = "ISO8601_SECOND", RegexPattern = @"(?:%{SECOND}|60)"},
                    new RegexAlias
                    {
                        Name = "TIMESTAMP_ISO8601",
                        RegexPattern =
                            @"%{YEAR}-%{MONTHNUM}-%{MONTHDAY}[T ]%{HOUR}:?%{MINUTE}(?::?%{SECOND})?%{ISO8601_TIMEZONE}?"
                    },
                    new RegexAlias {Name = "DATE", RegexPattern = @"%{DATE_US}|%{DATE_EU}"},
                    new RegexAlias {Name = "DATESTAMP", RegexPattern = @"%{DATE}[- ]%{TIME}"},
                    new RegexAlias {Name = "TZ", RegexPattern = @"(?:[PMCE][SD]T)"},
                    new RegexAlias
                    {
                        Name = "DATESTAMP_RFC822",
                        RegexPattern = @"%{DAY} %{MONTH} %{MONTHDAY} %{YEAR} %{TIME} %{TZ}"
                    },
                    new RegexAlias
                    {
                        Name = "DATESTAMP_OTHER",
                        RegexPattern = @"%{DAY} %{MONTH} %{MONTHDAY} %{TIME} %{TZ} %{YEAR}"
                    },

                    // Syslog Dates
                    new RegexAlias {Name = "SYSLOGTIMESTAMP", RegexPattern = @"%{MONTH} +%{MONTHDAY} %{TIME}"},
                    new RegexAlias {Name = "PROG", RegexPattern = @"(?:[\w._/-]+)"},
                    new RegexAlias {Name = "SYSLOGPROG", RegexPattern = @"%{PROG:program}(?:\[%{POSINT:pid}\])?"},
                    new RegexAlias {Name = "SYSLOGHOST", RegexPattern = @"%{IPORHOST}"},
                    new RegexAlias {Name = "SYSLOGFACILITY", RegexPattern = @"<%{POSINT:facility}.%{POSINT:priority}>"},
                    new RegexAlias
                    {
                        Name = "HTTPDATE",
                        RegexPattern = @"%{MONTHDAY}/%{MONTH}/%{YEAR}:%{TIME} %{INT:ZONE}"
                    },

                    // Shortcuts
                    new RegexAlias {Name = "QS", RegexPattern = @"%{QUOTEDSTRING}"},

                    // Log Formats
                    new RegexAlias
                    {
                        Name = "SYSLOGBASE",
                        RegexPattern =
                            @"%{SYSLOGTIMESTAMP:timestamp} (?:%{SYSLOGFACILITY} )?%{SYSLOGHOST:logsource} %{SYSLOGPROG}:"
                    },
                    new RegexAlias
                    {
                        Name = "COMBINEDAPACHELOG",
                        RegexPattern =
                            @"%{IPORHOST:clientip} %{USER:ident} %{USER:auth} \[%{HTTPDATE:timestamp}\] ""%{WORD:verb} %{URIPATHPARAM:request} HTTP/%{NUMBER:httpversion}"" %{NUMBER:response} (?:%{NUMBER:bytes}|-) (?:""(?:%{URI:referrer}|-)""|%{QS:referrer}) %{QS:agent}"
                    }
                };
            }
        }
    }
}
