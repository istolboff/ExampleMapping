using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ExampleMapping.Specs.SpecFlow
{
    internal sealed class MismatchedRow : RowMatchingResult, IEquatable<MismatchedRow>
    {
        public MismatchedRow(string rowKey, int rowIndex, IReadOnlyCollection<MismatchedColumn> mismatchedColumns)
            : base(rowKey, rowIndex)
        {
            Contract.Requires(mismatchedColumns.Any());

            _mismatchedColumns = mismatchedColumns;
        }

        public bool Equals(MismatchedRow other)
        {
            return 
                !ReferenceEquals(other, null) && 
                RowKey == other.RowKey && 
                _mismatchedColumns.SequenceEqual(other._mismatchedColumns);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MismatchedRow);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return DescribeRow(string.Join(", ", _mismatchedColumns));
        }

        private readonly IReadOnlyCollection<MismatchedColumn> _mismatchedColumns;
    }
}
