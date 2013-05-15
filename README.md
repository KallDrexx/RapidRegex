RapidRegex
==========

RapidRegex is a library to make the creation and maintenance of regular expressions easier.  
It does this by allowing you to create regular expression aliases.  You can then use these aliases
to create an aliased regular expression pattern, that will resolve to a real regular expression 
to be used against C#'s Regex engine.

This project was inspired by [Grok](https://code.google.com/p/semicomplete/wiki/Grok).  I wanted to use Grok-like
syntax for regular expressions in an Asp.Net web site and did not find an alternative that would work in .Net, 
so I created one.

The power of this library is to make it easy for end users to create regular expressions to 
perform tasks without deep knowledge of regular expressions, and without being able to read and repeat complicated 
regular expressions.

Example
-------

Say we want users to be able to easily form regular expressions that involve IP addresses.
A standard regular expression for an IP address would look like:

	\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b

To set up an alias for this, we can create an alias resolver via:

    var alias = new RegexAlias
    {
        Name = "IPAddress",
        RegexPattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
    };
	
	var resolver = new RegexAliasResolver(new RegexAlias[] { alias });

Now that we have a resolver we are able to convert patterns containing aliases into proper regular expression patterns.

	const string pattern = "connection from %{IPAddress}";
    const string test1 = "connection from 192.168.0.1";
    const string test2 = "connection from 555.555.555.555";
            
    // Resolve the pattern into becomes "connection from \b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
    var regexPattern = resolver.ResolveToRegex(pattern);

    // Run the regex
    var match1 = Regex.Match(test1, regexPattern);
    var match2 = Regex.Match(test2, regexPattern);

    Assert.IsTrue(match1.Success);
    Assert.IsFalse(match2.Success);

Aliases are chainable to allow the breaking down of complicated regular expressions into smaller pieces.
To keep this example going, we can define an IP address as 4 sets of digits (between 0-255) with a 
period in front of them, such as:

    var alias = new RegexAlias
    {
        Name = "IPDigit",
        RegexPattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"
    };

    var alias2 = new RegexAlias
    {
        Name = "IPAddress",
        RegexPattern = @"%{IPDigit}\.%{IPDigit}\.%{IPDigit}\.%{IPDigit}"
    };

    var resolver = new RegexAliasResolver(new[] { alias, alias2 });

    const string pattern = "connection from %{IPAddress}";
    const string test1 = "connection from 192.168.0.1";
    const string test2 = "connection from 555.555.555.555";

    // Resolve the pattern into becomes "connection from \b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b"
    var regexPattern = resolver.ResolveToRegex(pattern);

    // Run the regex
    var match1 = Regex.Match(test1, regexPattern);
    var match2 = Regex.Match(test2, regexPattern);

    Assert.IsTrue(match1.Success);
    Assert.IsFalse(match2.Success);
	
Defaults
--------

RapidRegex comes with a set of base aliases that can be used if desired.  The base
aliases must be explicitely enabled, so to use them you can instantiate a resolver
like so:

	var resolver = new RegexAliasResolver(BaseAliases.All);

If you want to use custom aliases alongside base aliases, you can do that by
concatenating the custom aliases with the base aliases like:

    var aliases = BaseAliases.All.Concat(new[] {alias, alias2});
    var testing = new RegexAliasResolver(aliases);

The default aliases include:

    HEX \b#?([a-f0-9]{6}|[a-f0-9]{3})\b
    Email ([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})

The following aliases were pulled from grok's base and are usable in RapidRegex

    USERNAME [a-zA-Z0-9_-]+
    USER %{USERNAME}
    INT (?:[+-]?(?:[0-9]+))
    BASE10NUM (?<![0-9.+-])(?>[+-]?(?:(?:[0-9]+(?:\.[0-9]+)?)|(?:\.[0-9]+)))
    NUMBER (?:%{BASE10NUM})
    BASE16NUM (?<![0-9A-Fa-f])(?:[+-]?(?:0x)?(?:[0-9A-Fa-f]+))
    BASE16FLOAT \b(?<![0-9A-Fa-f.])(?:[+-]?(?:0x)?(?:(?:[0-9A-Fa-f]+(?:\.[0-9A-Fa-f]*)?)|(?:\.[0-9A-Fa-f]+)))\bPOSINT \b(?:[0-9]+)\b
    WORD \b\w+\b
    NOTSPACE \S+
    SPACE \s*
    DATA .*?
    GREEDYDATA .*
    QUOTEDSTRING (?:(?<!\\)(?:"(?:\\.|[^\\"])*"|(?:'(?:\\.|[^\\'])*')|(?:`(?:\\.|[^\\`])*`)))
    MAC (?:%{CISCOMAC}|%{WINDOWSMAC}|%{COMMONMAC})
    CISCOMAC (?:(?:[A-Fa-f0-9]{4}\.){2}[A-Fa-f0-9]{4})
    WINDOWSMAC (?:(?:[A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2})
    COMMONMAC (?:(?:[A-Fa-f0-9]{2}:){5}[A-Fa-f0-9]{2})
    IP (?<![0-9])(?:(?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2}))(?![0-9])
    HOSTNAME \b(?:[0-9A-Za-z][0-9A-Za-z-]{0,62})(?:\.(?:[0-9A-Za-z][0-9A-Za-z-]{0,62}))*(\.?|\b)
    HOST %{HOSTNAME}
    IPORHOST (?:%{HOSTNAME}|%{IP})
    HOSTPORT (?:%{IPORHOST=~/\./}:%{POSINT})
    PATH (?:%{UNIXPATH}|%{WINPATH})
    UNIXPATH (?<![\w\\/])(?:/(?:[\w_%!$@:.,-]+|\\.)*)+LINUXTTY (?:/dev/pts/%{POSINT})
    BSDTTY (?:/dev/tty[pq][a-z0-9])
    TTY (?:%{BSDTTY}|%{LINUXTTY})
    WINPATH (?:[A-Za-z]+:|\\)(?:\\[^\\?*]*)+
    URIPROTO [A-Za-z]+(\+[A-Za-z+]+)?
    URIHOST %{IPORHOST}(?::%{POSINT:port})?
    URIPATH (?:/[A-Za-z0-9$.+!*'(),~:URIPARAM \?[A-Za-z0-9$.+!*'(),~
    URIPATHPARAM %{URIPATH}(?:%{URIPARAM})?
    URI %{URIPROTO}://(?:%{USER}(?::[^@]*)?@)?(?:%{URIHOST})?(?:%{URIPATHPARAM})?
    MONTH \b(?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\b
    MONTHNUM (?:0?[1-9]|1[0-2])
    MONTHDAY (?:3[01]|[1-2]?[0-9]|0?[1-9])
    DAY (?:Mon(?:day)?|Tue(?:sday)?|Wed(?:nesday)?|Thu(?:rsday)?|Fri(?:day)?|Sat(?:urday)?|Sun(?:day)?)
    YEAR [0-9]+HOUR (?:2[0123]|[01][0-9])
    MINUTE (?:[0-5][0-9])SECOND (?:(?:[0-5][0-9]|60)(?:[.,][0-9]+)?)
    TIME (?!<[0-9])%{HOUR}:%{MINUTE}(?::%{SECOND})(?![0-9])DATE_US %{MONTHNUM}[/-]%{MONTHDAY}[/-]%{YEAR}
    DATE_EU %{YEAR}[/-]%{MONTHNUM}[/-]%{MONTHDAY}
    ISO8601_TIMEZONE (?:Z|[+-]%{HOUR}(?::?%{MINUTE}))
    ISO8601_SECOND (?:%{SECOND}|60)
    TIMESTAMP_ISO8601 %{YEAR}-%{MONTHNUM}-%{MONTHDAY}[T ]%{HOUR}:?%{MINUTE}(?::?%{SECOND})?%{ISO8601_TIMEZONE}?
    DATE %{DATE_US}|%{DATE_EU}
    DATESTAMP %{DATE}[- ]%{TIME}
    TZ (?:[PMCE][SD]T)
    DATESTAMP_RFC822 %{DAY} %{MONTH} %{MONTHDAY} %{YEAR} %{TIME} %{TZ}
    DATESTAMP_OTHER %{DAY} %{MONTH} %{MONTHDAY} %{TIME} %{TZ} %{YEAR}
    SYSLOGTIMESTAMP %{MONTH} +%{MONTHDAY} %{TIME}
    PROG (?:[\w._/-]+)
    SYSLOGPROG %{PROG:program}(?:\[%{POSINT:pid}\])?
    SYSLOGHOST %{IPORHOST}
    SYSLOGFACILITY <%{POSINT:facility}.%{POSINT:priority}>
    HTTPDATE %{MONTHDAY}/%{MONTH}/%{YEAR}:%{TIME} %{INT:ZONE}
    QS %{QUOTEDSTRING}
    SYSLOGBASE %{SYSLOGTIMESTAMP:timestamp} (?:%{SYSLOGFACILITY} )?%{SYSLOGHOST:logsource} %{SYSLOGPROG}:
    COMBINEDAPACHELOG %{IPORHOST:clientip} %{USER:ident} %{USER:auth} \[%{HTTPDATE:timestamp}\] "%{WORD:verb} %{URIPATHPARAM:request} HTTP/%{NUMBER:httpversion}" %{NUMBER:response} (?:%{NUMBER:bytes}|-) (?:"(?:%{URI:referrer}|-)"|%{QS:referrer}) %{QS:agent}