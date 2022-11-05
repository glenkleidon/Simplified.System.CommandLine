using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.RegularExpressions;

namespace Simplified.System.Commandline
{
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

        bool AllowEmpty { get; set; }

        void AddAlias(string alias);
        void RemoveAlias(string alias);
        string ErrorMessage { get; set; }
        bool IsErrorOrEmpty { get; }
        bool Empty { get; set; }
        ValidateSymbolResult<ArgumentResult> Validator { get; set; }
        void ConnectValidator();
        Type typeOf { get; }
        IAsTypedValue NullOrValue { get; }

    }
}
