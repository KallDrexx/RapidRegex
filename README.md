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