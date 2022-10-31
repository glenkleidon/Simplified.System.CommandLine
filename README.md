# Simplified.System.CommandLine
A simplifying wrapper for the Microsoft System.CommandLine package.

## What does the System.CommandLine package do?

Microsoft currently has a new Command line Interface for Console Apps under development.

As of November 2022, this is still in beta `2.0.0-beta4.22272.1` [Command Line Api](https://github.com/dotnet/command-line-api).

The project Github says:

> The System.CommandLine library provides functionality that is commonly needed by command-line apps, such as parsing the command-line input and displaying help text.

This implementation is perfect for compatibility with the CLI and provides outstanding support for this.

This is an exceedingly powerful implementation: consequently it is also  __*extremely*__ complex and a little challenging for everyday users.

For example, to read just one parameter from the command line you can use this code:

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

For me, the workflow here is a little hard to follow.  And, it is not that clear exactly where you can access the returned values.  It appears that the handler is the only place that you can do this at this time.

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
There is also a more powerful [ParseArgument](https://learn.microsoft.com/en-us/dotnet/api/system.commandline.parsing.parseargument-1) to give greater control of the result. 

## What does this Package do?

The purpose of this package, as the name suggests, is to provide a simpler approach for the common use case: __**Read in parameters, validate them, and make it available to use**__

The power of the underlying `System.CommandLine` is still available for more advanced handling.


# Getting Started.

TODO: <nuget package>

## Single Argument Example
### Read an IP address from the command line (validating that is in the correct format)
The simplest form of this is:
```c#
    var ipAddress = new ParameterInfo<string>("IP Address", "The IP of the Computer to connect to.")
        {
            ValidationMessage = $"Must be a valid IP4 address",
            ValidationExpression = @"((\d){1,3}\.){3}\d{1,3}\b"
        };

    SimplifiedCommandLineHandler.ExtractParameter(args, ipAddress);

    if (ipAddress.IsErrorOrEmpty)
        Console.Error.WriteLine($"Cant connect to an invalid IP Address");
    else
        Console.WriteLine($"Connecting to host '{ipAddress.Value}'");
```
If there is a validation failure, you get a message like this:
    
    ![image](./resources/InvalidIP_sc.png?raw=true)
    
You see that Help messages are also available.
    
A slight variation using more a fluent syntax with the `FirstParameter` method is also available ((mainly for use with (Model Binding)[https://learn.microsoft.com/en-us/dotnet/standard/commandline/model-binding#custom-validation-and-binding])

```c#
using Simplified.System.Commandline;

var ipAddress = SimplifiedCommandLineHandler
     .FirstParameter(args,
         new ParameterInfo<string>("IP Address", "The IP of the Computer to connect to.")
         {
             ValidationMessage = $"Must be a valid IP4 address",
             ValidationExpression = @"((\d){1,3}\.){3}\d{1,3}\b"
         });

if (ipAddress.IsErrorOrEmpty)
    Console.Error.WriteLine($"Cant connect to an invalid IP Address");
else
    Console.WriteLine($"Connecting to host '{ipAddress.Value}'");

```
In this example, as we have no action methods, ie it doesnt try to act on the results, there is no need to run asynchronously.
    


## Multi-Argument Example

## Adding Options 
Not yet implemented... use CommandLine Options for now.
    
## Modifying Help
Not yet implemented... use CommandLine Options for now.
   
## Alternate Validators 
The static `RegExArgumentValidator<T>` class supports `Regex` and the __No Validator__ case. 
    




