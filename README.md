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

Alias Configuration
-------------------

RapidRegex requires you to build `RegexAlias` data structures in order to define the different
aliases it will parse.  It does not force you a specific format for how to form these data structures.  
This gives you the flexibility in how you store and edit aliases.  For example, you could store
them in a database (for editing via a web page) or even to form a centralized web service 
that can be called to retrieve the `RegexAlias` structures.

To make it easier to get up and running, the project includes a `BasicAliasConfigReader` class,
which allows you to read a basic alias configuration from a file or a stream.  The basic alias
configuration format is:

    # This is a Comment
    AliasName My Regex Pattern
    SecondAlias [a-z]+

The first character of a valid alias must not be a space, and the alias name must be one word
with no spaces in between.  All characters after the first space in the line will be counted
as part of the regular expression pattern.  It allows one alias per line.

RapidRegex comes with a set of basic alias configurations.  

