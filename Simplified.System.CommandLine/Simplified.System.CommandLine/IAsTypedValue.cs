using System;

namespace Simplified.System.Commandline
{
    public interface IAsTypedValue
    {
        DateTime? AsDateTime();
        double? AsDouble();
        decimal? AsDecimal();
        int? AsInt();
        long? AsInt64();
        string AsString();
        bool? AsBool();
        char? AsChar();
        bool IsNull();
        bool IsNullOrEmpty();
        Type TypeOf();
        
    }
}
