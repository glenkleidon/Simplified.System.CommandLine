using Simplified.System.Commandline;

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
    }
}