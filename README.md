# Simplified.System.CommandLine
A simplifying wrapper for the Microsoft System.CommandLine package.

## What does the System.CommandLine package do.

Microsoft currently has a new Command line Interface for Console Apps under development.  As of November 2022, this is still in beta `2.0.0-beta4.22272.1` [Command Line Api](https://github.com/dotnet/command-line-api).

The project Github says:

> The System.CommandLine library provides functionality that is commonly needed by command-line apps, such as parsing the command-line input and displaying help text.

This implementation is perfect for compatibility with the CLI and provides outstanding support for this.

This is an exceedingly powerful implementation: consequently it is also  _*extremely*_ complex and unweildy to use.

For example to read 1 parameter from the command line you need can use this code:
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

The workflow in this is a little hard to follow and it is not that clear exactly where you can access the returned values.  It appears that the handler is the only place that you can do this at this time.

If you want to validate the results, you can certainly do this in the handler but there are also `Validators` that can be used like this.
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

## What does this Package do.

Due to the complexity of the `System.CommandLine` package, everyday users are likely to find it difficult to follow and use.  The purpose of this package, as the name suggets is to provide a simpler approach to common validation tasks.

# Getting Started.

TODO: <nuget package>

## Read an IP address from the command line (validating that is in the correct format)

```c#
using Simplified.System.Commandline;

  var parametersInfo = new List<ISimplifiedCommandLineParameterInfo>()
  { 
      new SimplifiedCommandLineParameterInfo<string>("Host") 
      {
          Index = 0,
          Description = "The name or IP of the Computer to connect to.",
          ValidationMessage =  $"Must be a valid Host name, IP4, IP6 address",
          ValidationExpression = HostNameValidator

      }
  };
  var commandLineParams = SimplifiedCommandLineHandler.ExtractParameters(args, parametersInfo);


```







