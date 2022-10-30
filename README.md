# Simplified.System.CommandLine
A simplifying wrapper for the Microsoft System.CommandLine package.

## What does the System.CommandLine package do.

Microsoft currently has a new Command line Interface for Console Apps under development.

As of November 2022, this is still in beta `2.0.0-beta4.22272.1` [Command Line Api](https://github.com/dotnet/command-line-api).

The project Github says:

> The System.CommandLine library provides functionality that is commonly needed by command-line apps, such as parsing the command-line input and displaying help text.

This implementation is perfect for compatibility with the CLI and provides outstanding support for this.

This is an exceedingly powerful implementation: consequently it is also  __*extremely*__ complex and a little challenging for everyday users.

For example to, read 1 parameter from the command line you need can use this code:

[See full Example](https://learn.microsoft.com/en-us/dotnet/standard/commandline/define-commands#define-arguments)

```c#
    var messageArgument = new Argument<string>
        ("message", "An argument that is parsed as a string.");

    var rootCommand = new RootCommand();
    rootCommand.Add(messageArgument);

    rootCommand.SetHandler((delayArgumentValue, messageArgumentValue) =>
        {
            Console.WriteLine($"<message> argument = {messageArgumentValue}");
        },
        delayArgument, messageArgument);

    await rootCommand.InvokeAsync(args);
```

For me, the workflow here is a little hard to follow.  And, it is not that clear exactly where you can access the returned values.

It appears that the handler is the only place that you can do this at this time.

You can certainly validate the results in the Handler, but there is support for specific `Validators` that can be used like this:

[See full description here](https://learn.microsoft.com/en-us/dotnet/standard/commandline/define-commands#define-arguments)

```c#
  var delayOption = new Option<int>("--delay");
  delayOption.AddValidator(result =>
  {
      if (result.GetValueForOption(delayOption) < 1)
      {
          result.ErrorMessage = "Must be greater than 0";
      }
  });
```

## What does this Package do?

The purpose of this package, as the name suggests, is to provide a simpler approach for the common use case: __**Read in a parameter, validate it, and make it available for use**__


# Getting Started.

TODO: <nuget package>

## Read an IP address from the command line (validating that is in the correct format)

```c#
using Simplified.System.Commandline;

var ipAddress = SimplifiedCommandLineHandler
     .FirstParameter(args,
         new ParameterInfo<string>("IP Address")
         {
             Description = "The IP of the Computer to connect to.",
             ValidationMessage = $"Must be a valid IP4 address",
             ValidationExpression = @"((\d){1,3}\.){3}\d{1,3}\b"

         });

if (ipAddress.ErrorOrEmpty)
    Console.Error.WriteLine($"Cant connect to an invalid IP Address");
else
    Console.WriteLine($"Connecting to host '{ipAddress}'");

   
    


```







