namespace Internal.AspNetCore.QueryGenerator.Model
{
    internal class QueryDefinition
    {
        public string Name { get; set; }
        public QueryGrouping Grouping { get; set; } = QueryGrouping.None;
        public string QueryText { get; set; }
    }

    internal enum QueryGrouping
    {
        None,
        Area,
    }
}