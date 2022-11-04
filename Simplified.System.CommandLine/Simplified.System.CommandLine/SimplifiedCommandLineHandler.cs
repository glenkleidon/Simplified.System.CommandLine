using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text.RegularExpressions;
using static Simplified.System.Commandline.SimplifiedCommandLineHandler;

namespace Simplified.System.Commandline
{
    public enum ParamId { Name, Index, TypeName, Alias };
    public static class SimplifiedCommandLineParameterExtensions
    {
        public static string AsFormatId(this ParamId formatIndex)
        {
            return $"{{(int)formatIndex}}";
        }
    }
    public interface IParameterInfo
    {
        IEnumerable<string> Aliases { get; }
        Argument Arg { get; }
        string Description { get; set; }
        int Index { get; set; }
        string Name { get; set; }
        RegexOptions ValidationOptions { get; set; }
        string ValidationExpression { get; set; }
        Regex ValidationRegex { get; set; }
        int ValidationMaxMatches { get; set; }
        string ValidationMessage { get; set; }

        void AddAlias(string alias);
        void RemoveAlias(string alias);
        string ErrorMessage { get; set; }
        bool IsErrorOrEmpty { get; }
        bool Empty { get; set; }
        ValidateSymbolResult<ArgumentResult> Validator { get; set; }
        void ConnectValidator();

    }

    public class ParameterInfo<T> : IParameterInfo
    {
        private const string ValidationPrefix = "<{0}> (Param #{1}:{2})";
        private const string ValidationPrefixIncorrect = ValidationPrefix + " incorrect";

        public T Value { get; set; }

        private Regex regex = null;

        public RegexOptions ValidationOptions { get; set; } = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        public string ValidationExpression
        {
            get => (regex == null) ? String.Empty : regex.ToString();
            set => ValidationRegex = new Regex(value, ValidationOptions);
        }
        public ParameterInfo()
        {
            Empty = true;
            Name = $"{typeof(T).Name}Param{Index + 1}";
            ErrorMessage = "";
            commandLineArgument = new Argument<T>();
        }
        public ParameterInfo(string name, string description = "")
        {
            Empty = true;
            ErrorMessage = "";
            Name = name;
            Description = description;
            commandLineArgument = new Argument<T>(name, description);
        }
        private List<string> aliases = new List<string>();
        public IEnumerable<string> Aliases { get => aliases; }
        public void AddAlias(string alias)
        {
            if (alias != null && !Aliases.Any(a => a.Equals(alias))) aliases.Add(alias);
        }
        public void RemoveAlias(string alias)
        {
            if (alias != null) aliases.Remove(alias);
        }
        public int Index { get; set; } = 0;
        public string Name { get; set; }
        private Argument<T> commandLineArgument;
        public Argument<T> CommandLineArgument
        {
            get
            {
                commandLineArgument.Name = Name;
                commandLineArgument.Description = Description;
                return commandLineArgument;
            }
            set => commandLineArgument = value ?? new Argument<T>(Name, Description);
        }

        private string description = ValidationPrefix;
        public string Description { get => FormatText(description); set => description = value; }

        private string FormatText(string text)
        {
            return String.Format(text, Name, Index + 1, typeof(T).Name,
                  String.Join(",", Aliases));
        }

        public void ConnectValidator()
        {
            if (Validator==null)
                RegExArgumentValidator<T>.SetValidator(this);
        }

        private string validationMessage = $"{ValidationPrefixIncorrect}.";
        public string ValidationMessage
        {
            get => FormatText(validationMessage);
            set => validationMessage = $"{ValidationPrefixIncorrect}:\r\n    {value}";
        }

        Argument IParameterInfo.Arg => CommandLineArgument;

        public Regex ValidationRegex
        {
            get => regex;
            set => regex = value;
        }

        public bool Empty { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsErrorOrEmpty => Empty || ErrorMessage != String.Empty;

        public ValidateSymbolResult<ArgumentResult> Validator { get; set; }
        public int ValidationMaxMatches { get; set; } = 1;
    }

    public struct FirstParamResult
    {
        private bool errorOrEmpty;
        private string paramValue;

        public FirstParamResult(string value, bool errorOrEmpty)
        {
            this.errorOrEmpty = errorOrEmpty;
            paramValue = value ?? String.Empty;

        }
        public string Value { get => paramValue; }
        public bool IsErrorOrEmpty { get => errorOrEmpty; }
        public static implicit operator string(FirstParamResult firstParamResult) => firstParamResult.Value;
    }


    public class SimplifiedCommandLineHandler
    {
        public static FirstParamResult FirstParameter(string[] args, ParameterInfo<string> argInfo)
        {
            var list = new List<IParameterInfo>();
            list.Add(argInfo);
            ExtractParameters(args, list);
            return new FirstParamResult(argInfo?.Value ?? String.Empty, !argInfo?.IsErrorOrEmpty ?? true);
        }



        public static RootCommand ExtractParameters(string[] args, IEnumerable<IParameterInfo> argInfo)
        {
            var cmd = new RootCommand();
            var sorted = argInfo.OrderBy(i => i.Index);
            foreach (var info in sorted)
            {
                if (info.Arg != null)
                {
                    info.ConnectValidator();
                    cmd.AddArgument(info.Arg);
                }
            }
            
                
            cmd.Invoke(args);
            return cmd;
        }

        public static RootCommand ExtractParameter(string[] args, IParameterInfo argInfo)
        {
            var argInfos = new List<IParameterInfo>();
            argInfos.Add(argInfo);
            return ExtractParameters(args, argInfos);
        }

        public static class RegExArgumentValidator<T>
        {
            public static void SetValidator(ParameterInfo<T> info, string pattern, RegexOptions options = RegexOptions.Compiled)
            {
                info.ValidationOptions = options;
                info.ValidationExpression = pattern;
                SetValidator(info);
            }

            public static void SetValidator(ParameterInfo<T> info)
            {
                info.Validator =
                arg =>
                     {
                         info.Empty = true;
                         info.Value = arg.GetValueOrDefault<T>();
                         if (info.Value != null)
                         {

                             if (info.ValidationExpression == null)
                                 info.Empty = false;
                             else
                             {
                                 var cValue = Convert.ToString(info.Value);
                                 if (cValue != null)
                                 {
                                     var matches = info.ValidationRegex?.Matches(cValue);
                                     if (matches?.Count == 1)
                                         info.Empty = false;
                                     else
                                     {
                                         info.ErrorMessage = $"{info.ValidationMessage} [Regex Validator]";
                                         arg.ErrorMessage = info.ErrorMessage;
                                     }
                                 }
                             }
                         }
                     };
                info.CommandLineArgument?.AddValidator(info.Validator);
                

            }

        }


    }
}
