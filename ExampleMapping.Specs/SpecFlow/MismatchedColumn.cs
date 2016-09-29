using System;

namespace ExampleMapping.Specs.SpecFlow
{
    internal sealed class MismatchedColumn : IEquatable<MismatchedColumn>
    {
        public MismatchedColumn(string propertyName, string expectedValue, string actualValue)
        {
            _propertyName = propertyName;
            _expectedValue = expectedValue;
            _actualValue = actualValue;
        }

        public bool Equals(MismatchedColumn other)
        {
            return
                !ReferenceEquals(other, null) &&
                _propertyName == other._propertyName &&
                ValuesAreEqual(_expectedValue, other._expectedValue) &&
                ValuesAreEqual(_actualValue, other._actualValue);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MismatchedColumn);
        }

        public override int GetHashCode()
        {
            return ((_propertyName.GetHashCode() * 397) ^ (_expectedValue ?? string.Empty).GetHashCode()) * 397 ^ (_actualValue ?? string.Empty).GetHashCode();
        }

        public override string ToString()
        {
            return $"{_propertyName} {{Expected: {_expectedValue}; Actual: {_actualValue}}}";
        }

        private static bool ValuesAreEqual(string leftValue, string rightValue)
        {
            if (leftValue == rightValue)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(leftValue) || string.IsNullOrWhiteSpace(rightValue))
            {
                return leftValue == "NULL" || rightValue == "NULL";
            }

            return false;
        }

        private readonly string _propertyName;
        private readonly string _expectedValue;
        private readonly string _actualValue;
    }
}
