using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ExampleMapping.Specs.Miscellaneous;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ExampleMapping.Specs.SpecFlow
{
    internal static class TableExtensions
    {
        public static TableMatchingResult Match<TRowType>(this TableRows @this, IEnumerable<TRowType> actualObjects, IDictionary<string, string> nameDecoders = null)
        {
            Contract.Requires(@this != null);
            Contract.Requires(actualObjects != null);
            return new TableMatchingResult(ListDataMismatches(@this, actualObjects, (actualRow, columnName) => actualRow.GetPropertyValueByName(columnName.Replace(" ", string.Empty))));
        }

        public static IReadOnlyCollection<T> ToReadOnlySet<T>(this Table @this)
        {
            Contract.Requires(@this != null);

            return @this.CreateSet<T>().ToList();
        }

        private static IReadOnlyCollection<RowMatchingResult> ListDataMismatches<TRowType>(
            TableRows expectedRows,
            IEnumerable<TRowType> actualRows,
            Func<TRowType, string, object> getCellValue)
        {
            var firstColumnName = expectedRows.First().Keys.First();

            var expectedRowsWithIndexAndKey = expectedRows.Select((row, index) => new { row, index, key = row[0] }).AsImmutable();
            var actualRowsWithKey = actualRows.Select((row, index) => new { row, index, key = getCellValue(row, firstColumnName).ToString() }).AsImmutable();

            var mismatchedAndMissingRows =
                from expectedRow in expectedRowsWithIndexAndKey
                join actualRow in actualRowsWithKey on expectedRow.key equals actualRow.key into matchedActualRows
                from matchedActualRow in matchedActualRows.DefaultIfEmpty()
                let rowMatchingResult = matchedActualRow != null 
                    ? MatchRows(expectedRow.row, matchedActualRow.row, expectedRow.index, getCellValue)
                    : new MissingRow(expectedRow.key, expectedRow.index)
                where rowMatchingResult != null
                select rowMatchingResult;

            var unnecessaryRows =
                from actualRow in actualRowsWithKey
                where expectedRowsWithIndexAndKey.All(row => row.key != actualRow.key)
                select new UnnecessaryRow(actualRow.key);

            return mismatchedAndMissingRows.Concat(unnecessaryRows).AsImmutable();
        }

        private static RowMatchingResult MatchRows<TRowType>(
            TableRow expectedRow,
            TRowType actualRow,
            int rowIndex,
            Func<TRowType, string, object> getCellValue)
        {
            var mismatchedColumns =
                expectedRow.Keys.Zip(expectedRow.Values, (key, value) => new { key, value })
                .Select(item => MatchColumnValues(item.key, item.value, actualRow, getCellValue))
                .Where(item => item != null)
                .AsImmutable();

            var rowKey = expectedRow.Values.First();
            return mismatchedColumns.Any() ? new MismatchedRow(rowKey, rowIndex, mismatchedColumns) : null;
        }

        private static MismatchedColumn MatchColumnValues<TRowType>(
            string columnName,
            string expectedValue,
            TRowType actualRow,
            Func<TRowType, string, object> getCellValue)
        {
            var actualValue = getCellValue(actualRow, columnName);

            if (actualValue == null || actualValue == DBNull.Value)
            {
                return
                    string.IsNullOrEmpty(expectedValue)
                        ? null
                        : new MismatchedColumn(columnName, expectedValue, "NULL");
            }

            var actualValueText = actualValue.ToString().TrimEnd();

            return expectedValue.Equals(actualValueText, StringComparison.OrdinalIgnoreCase) 
                ? null 
                : new MismatchedColumn(columnName, expectedValue, actualValueText);
        }
    }
}
