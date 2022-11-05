using Simplified.System.Commandline;
using System.Net.WebSockets;

namespace Simplified.System.CommandLine.Tests
{
    public class AsTypedValueTests
    {
        public List<IAsTypedValue> Sut { get; }
        public AsTypedValueTests()
        {
            var now = DateTime.Now;

            Sut = new List<IAsTypedValue>()
            {
                new AsTypedValue<int>(int.MaxValue),
                new AsTypedValue<int>(int.MinValue),
                new AsTypedValue<int>(0),
                new AsTypedValue<int?>(null),


                new AsTypedValue<bool>(true),
                new AsTypedValue<bool>(false),
                new AsTypedValue<bool?>(null),

                new AsTypedValue<DateTime>(now),
                new AsTypedValue<DateTime>(DateTime.MinValue),
                new AsTypedValue<DateTime>(DateTime.MaxValue),
                new AsTypedValue<DateTime?>(null),

                new AsTypedValue<Int64>(Int64.MaxValue),
                new AsTypedValue<Int64>(Int64.MaxValue),
                new AsTypedValue<Int64>(0),
                new AsTypedValue<Int64?>(null),

                new AsTypedValue<Double>(Double.MaxValue),
                new AsTypedValue<Double>(Double.MinValue),
                new AsTypedValue<Double>(0),
                new AsTypedValue<Double?>(null),

                new AsTypedValue<Decimal>(Decimal.MaxValue),
                new AsTypedValue<Decimal>(Decimal.MinValue),
                new AsTypedValue<Decimal>(0),
                new AsTypedValue<Decimal?>(null),

                new AsTypedValue<char>((char)0),
                new AsTypedValue<char>((char)0x2665),
                new AsTypedValue<char?>(null),

            };
        }

        [Theory]
        [InlineData(typeof(char), 2)]
        [InlineData(typeof(char?), 1)]
        [InlineData(typeof(bool), 2)]
        [InlineData(typeof(bool?), 1)]
        [InlineData(typeof(int), 3)]
        [InlineData(typeof(int?), 1)]
        [InlineData(typeof(Int64), 3)]
        [InlineData(typeof(Int64?), 1)]
        [InlineData(typeof(double), 3)]
        [InlineData(typeof(double?), 1)]
        [InlineData(typeof(decimal), 3)]
        [InlineData(typeof(decimal?), 1)]
        [InlineData(typeof(DateTime), 3)]
        [InlineData(typeof(DateTime?), 1)]
        public void CheckParamsTypeOfEqualExpected(Type t, int count)
        {
            Assert.Equal(count, Sut.Count(v => v.TypeOf() == t));
        }



        [Theory]
        [InlineData("1.20")]
        [InlineData("0.0000002")]
        [InlineData("This is a string")]
        [InlineData("1")]        
        [InlineData(null)]
        public void StringParameterTypesReturnAsExpected(string value)
        {
            var sut = new AsTypedValue<string>(value) as IAsTypedValue;
            Assert.NotNull(sut);
            Assert.Equal(value, sut.AsString());
        }

        [Fact]
        public void StringCharParameterTypesReturnAsExpected()
        {
            var value = "" + (char)0x2661;
            var sut = new AsTypedValue<string>(value) as IAsTypedValue;
            Assert.NotNull(sut);
            Assert.Equal(value, sut.AsString());
        }
 
        [Fact]
        public void StringBoolFalseParameterTypesReturnAsExpected()
        {
            var value = bool.FalseString;
            var sut = new AsTypedValue<string>(value) as IAsTypedValue;
            Assert.NotNull(sut);
            Assert.Equal(value, sut.AsString());
        }

        [Fact]
        public void StringBoolTrueParameterTypesReturnAsExpected()
        {
            var value = bool.TrueString;
            var sut = new AsTypedValue<string>(value) as IAsTypedValue;
            Assert.NotNull(sut);
            Assert.Equal(value, sut.AsString());

        }

        

    }
}
