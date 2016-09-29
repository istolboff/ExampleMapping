namespace ExampleMapping.Specs.SpecFlow
{
    internal abstract class RowMatchingResult
    {
        protected RowMatchingResult(string rowKey)
        {
            RowKey = rowKey;
        }

        protected RowMatchingResult(string rowKey, int rowIndex)
            : this(rowKey)
        {
            _rowIndex = rowIndex;
        }

        public string RowKey { get; }

        public override int GetHashCode()
        {
            return RowKey.GetHashCode();
        }

        protected string DescribeRow(string details)
        {
            return $"Строка {(_rowIndex.HasValue ? ("№ " + (_rowIndex + 1) + " ") : string.Empty)}{RowKey}: {details}";
        }

        private readonly int? _rowIndex;
    }
}