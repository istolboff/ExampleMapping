using System;

namespace ExampleMapping.Specs.SpecFlow
{
    internal sealed class UnnecessaryRow : RowMatchingResult, IEquatable<UnnecessaryRow>
    {
        public UnnecessaryRow(string rowKey)
            : base(rowKey)
        {
        }

        public bool Equals(UnnecessaryRow other)
        {
            return !ReferenceEquals(other, null) && RowKey == other.RowKey;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UnnecessaryRow);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return DescribeRow("лишняя");
        }
    }
}
