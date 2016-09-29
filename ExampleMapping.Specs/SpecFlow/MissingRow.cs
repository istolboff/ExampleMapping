using System;

namespace ExampleMapping.Specs.SpecFlow
{
    internal sealed class MissingRow : RowMatchingResult, IEquatable<MissingRow>
    {
        public MissingRow(string rowKey, int index)
            : base(rowKey, index)
        {
        }

        public bool Equals(MissingRow other)
        {
            return !ReferenceEquals(other, null) && RowKey == other.RowKey;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MissingRow);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return DescribeRow("отсутствует");
        }
    }
}
