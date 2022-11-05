using Simplified.System.Commandline;

namespace Simplified.System.CommandLine.Tests
{
    public class ArgumentTests
    {
        [Fact]
        public void AnArgumentCanBeCreated()
        {
            var param1 = new ParameterInfo<string>("Param 1", "Description Of Param1");
            var args = new string[1] { "hello" };

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
        [InlineData("192.168.18.3", false, false)]
        [InlineData("192.168.1448.3", true, false)]
        [InlineData("", true, false)]
        [InlineData("", false, true)]
        public void AnIP4ArgumentCanBeValidatedUsingRegex(string value, bool hasError, bool allowEmpty)
        {
            var pattern = @"((\d){1,3}\.){3}\d{1,3}\b";
            var param1 = new ParameterInfo<string>("Param 1", "Regex Validation")
            {
                ValidationExpression = pattern,
                AllowEmpty = allowEmpty,
                ValidationMessage = "Not an IP4"
            };
            var args = new string[1] { value };
            SimplifiedCommandLineHandler.ExtractParameter(args, param1);
            Assert.Equal(hasError, param1.IsErrorOrEmpty);
            Assert.Equal(args[0], param1.Value);
            if (hasError)
            {
                if (string.IsNullOrWhiteSpace(args[0]))
                    Assert.Contains("[Empty Not Allowed]", param1.ErrorMessage);
                else
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

        [Fact]
        public void ThrowArgumentExceptionWhenMultipleParametersHaveUnsetIndexes()
        {
            var paramInfo = new List<IParameterInfo>() { new ParameterInfo<int>("Param 1") };
            var args = new string[] { "1", "2", "3" };
            SimplifiedCommandLineHandler.ExtractParameters(args, paramInfo);
            paramInfo.Add(new ParameterInfo<int>("Param 2") { Index = 1 });
            SimplifiedCommandLineHandler.ExtractParameters(args, paramInfo);
            paramInfo.Add(new ParameterInfo<int>("Param 3"));
            Assert.Throws<ArgumentException>(() => SimplifiedCommandLineHandler.ExtractParameters(args, paramInfo));
        }

        [Theory]
        [InlineData("", "", "", new int[] { 1, 2, 3 })]
        [InlineData("", "2001:8003:4c22:6701:e123:9395:629:8866", "", new int[] { 1, 3 })]
        [InlineData("2001:8003:4c22:6701:e123:9395:629:8866", "", "", new int[] { 2, 3 })]
        [InlineData("", "", "2001:8003:4c22:6701:e123:9395:629:8866", new int[] { 1, 2 })]
        [InlineData("2001:8003:4c22:6701:e123:9395:629:8866", "", "2001:8003:4c22:6701:e123:9395:629:8866", new int[] { 2 })]
        public void MultipleParametersCanBeValidated(string value1, string value2, string value3, int[] failures)
        {
            var pattern = @"([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}";
            var args = new string[3] { value1, value2, value3 };
            var paramInfo = new List<IParameterInfo>()
            {
                new ParameterInfo<string>("Param 1", "Regex Validation")
                {
                    Index = 0,
                    ValidationExpression = pattern,
                    ValidationMessage = "Not an IP6"
                },
                new ParameterInfo<string>("Param 2", "Required Validation")
                {
                    Index = 1,
                    AllowEmpty = false,
                    ValidationMessage = "Required"
                },
                new ParameterInfo<string>("Param 3", "Regex Validation")
                {
                    Index = 2,
                    ValidationExpression = pattern,
                    ValidationMessage = "Not an IP6"
                }
            };

            SimplifiedCommandLineHandler.ExtractParameters(args, paramInfo);

            foreach (var p in paramInfo)
            {
                var v = p.NullOrValue;
                var hasFailure = failures.Contains(p.Index + 1);
                Assert.Equal(hasFailure, p.IsErrorOrEmpty);
                Assert.Equal(args[p.Index], v.AsString());
                if (p.IsErrorOrEmpty)
                {
                    Assert.Contains($"Param #{p.Index + 1}", p.ErrorMessage);
                    Assert.Contains($"Param {p.Index + 1}", p.ErrorMessage);
                }
                else
                {
                    Assert.Empty(p.ErrorMessage);
                }
            }


        }
    }
}