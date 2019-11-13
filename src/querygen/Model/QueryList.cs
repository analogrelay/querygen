using System.Collections.Generic;

namespace Internal.AspNetCore.QueryGenerator.Model
{
    internal class QueryList
    {
        public IList<QueryArea> Areas { get; set; }
        public IList<QueryDefinition> Queries { get; set; }
    }
}
