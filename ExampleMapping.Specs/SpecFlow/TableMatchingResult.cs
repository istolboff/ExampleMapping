using System;
using System.Collections.Generic;
using System.Linq;

namespace ExampleMapping.Specs.SpecFlow
{
    internal sealed class TableMatchingResult
    {
        internal TableMatchingResult(IReadOnlyCollection<RowMatchingResult> rowMatchingResults)
        {
            RowMatchingResults = rowMatchingResults;
        }

        public IReadOnlyCollection<RowMatchingResult> RowMatchingResults { get; }

        public static implicit operator bool(TableMatchingResult @this)
        {
            return !@this.RowMatchingResults.Any();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, RowMatchingResults);
        }
    }
}
