using Simplified.System.Commandline;
using System.Text.RegularExpressions;

namespace Simplified.System.CommandLine.Tests
{
    public class ArgumentTests
    {
        [Fact]
        public void AnArgumentCanBeCreated()
        {
            var param1 = new ParameterInfo<string>("Param 1", "Description Of Param1");
            var args = new string[1] { "hello"};

            SimplifiedCommandLineHandler.ExtractParameter(args, param1);

            Assert.Equal("hello", param1.Value);

        }

        [Fact]
        public void AnArgumentCanBeValidated()
        {
            var param1 = new ParameterInfo<string>("Param 1", "Description Of Param1");
            var args = new string[1] { "hello" };

            SimplifiedCommandLineHandler.ExtractParameter(args, param1);

            Assert.Equal("hello", param1.Value);

        }
        [Theory]
        [InlineData("192.168.18.3", false)]
        [InlineData("192.168.1448.3", true)]
        public void AnIP4ArgumentCanBeValidatedUsingRegex(string value, bool hasError)
        {
            var pattern = @"((\d){1,3}\.){3}\d{1,3}\b";
            var param1 = new ParameterInfo<string>("Param 1", "Regex Validation")
            {
                ValidationExpression = pattern,
                ValidationMessage = "Not an IP4"
            };
            var args = new string[1] { value };
            SimplifiedCommandLineHandler.ExtractParameter(args, param1);
            Assert.Equal(hasError, param1.IsErrorOrEmpty);
            Assert.Equal(args[0], param1.Value);
            if (hasError)
            {
                Assert.Contains("[Regex Validator]", param1.ErrorMessage);
                Assert.Contains("incorrect", param1.ErrorMessage);
                Assert.Contains("Param #1", param1.ErrorMessage);
                Assert.Contains("Param 1", param1.ErrorMessage);
            }
            else
            {
                Assert.Empty(param1.ErrorMessage);
            }

        }
    }
}