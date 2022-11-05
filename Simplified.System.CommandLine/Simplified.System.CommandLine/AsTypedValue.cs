using System;

namespace Simplified.System.Commandline
{
    public sealed class AsTypedValue<T> : IAsTypedValue
    {
        private readonly T value;
        public AsTypedValue(T value)
        {
            this.value = value;
        }

        public string AsString()
        {
            return value?.ToString();
        }

        public int? AsInt()
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
                return Convert.ToInt32(value);
            int v;
            if (Int32.TryParse(AsString(), out v))
                return v;
            return null;
        }
        public Int64? AsInt64()
        {
            if (typeof(T) == typeof(Int64) || typeof(T) == typeof(Int64?))
                return Convert.ToInt64(value);
            Int64 v;
            if (Int64.TryParse(AsString(), out v))
                return v;
            return null;
        }

        public double? AsDouble()
        {
            if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
                return Convert.ToDouble(value);
            double v;
            if (Double.TryParse(AsString(), out v))
                return v;
            return null;
        }

        public DateTime? AsDateTime()
        {
            if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
                return Convert.ToDateTime(value);
            DateTime v;
            if (DateTime.TryParse(AsString(), out v))
                return v;
            return null;

        }

        public decimal? AsDecimal()
        {
            if (typeof(T) == typeof(Decimal) || typeof(T) == typeof(DateTime?))
                return Convert.ToDecimal(value);
            Decimal v;
            if (Decimal.TryParse(AsString(), out v))
                return v;
            return null;
        }

        public char? AsChar()
        {
            if (typeof(T) == typeof(char) || typeof(T) == typeof(char?))
                return Convert.ToChar(value);
            char v;
            if (char.TryParse(AsString(), out v))
                return v;
            return null;
        }

        public bool? AsBool()
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
                return Convert.ToBoolean(value);
            bool v;
            if (bool.TryParse(AsString(), out v))
                return v;
            return null;
        }

        public bool IsNull()
        {
            return AsString() == null;
        }
        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(AsString());
        }

        public Type TypeOf()
        {
            return typeof(T);
        }
    }
}
