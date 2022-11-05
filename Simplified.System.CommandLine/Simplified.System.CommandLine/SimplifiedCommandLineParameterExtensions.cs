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
}
