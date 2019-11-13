using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Internal.AspNetCore.QueryGenerator.Generator
{
    internal class QuerySet
    {
        private Dictionary<string, IList<GitHubQuery>> _queriesByArea = new Dictionary<string, IList<GitHubQuery>>();
        private List<GitHubQuery> _generalQueries = new List<GitHubQuery>();

        public IReadOnlyList<AreaQueries> QueriesByArea => _queriesByArea.Select(pair => new AreaQueries(pair.Key, pair.Value)).ToList();
        public IReadOnlyList<GitHubQuery> GeneralQueries => _generalQueries;

        public void AddAreaQuery(string area, GitHubQuery query)
        {
            if(!_queriesByArea.TryGetValue(area, out var queryList))
            {
                queryList = new List<GitHubQuery>();
                _queriesByArea[area] = queryList;
            }
            queryList.Add(query);
        }

        public void AddQuery(GitHubQuery query)
        {
            _generalQueries.Add(query);
        }
    }

    internal class AreaQueries
    {
        public AreaQueries(string areaName, IEnumerable<GitHubQuery> queries)
        {
            AreaName = areaName;
            Queries = queries.ToList();
        }

        public string AreaName { get; set; }
        public IReadOnlyList<GitHubQuery> Queries { get; set; }
    }
}
