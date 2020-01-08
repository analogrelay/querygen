using System;
using System.Collections.Generic;
using System.Linq;

namespace Internal.AspNetCore.QueryGenerator.Generator
{
    internal class QuerySet
    {
        private List<AreaQueries> _queriesByArea = new List<AreaQueries>();
        private List<GitHubQuery> _generalQueries = new List<GitHubQuery>();

        public IReadOnlyList<AreaQueries> QueriesByArea => _queriesByArea;
        public IReadOnlyList<GitHubQuery> GeneralQueries => _generalQueries;

        public AreaQueries EnsureArea(string areaName)
        {
            var area = _queriesByArea.FirstOrDefault(a => string.Equals(a.AreaName, areaName, StringComparison.OrdinalIgnoreCase));
            if (area == null)
            {
                area = new AreaQueries(areaName);
                _queriesByArea.Add(area);
            }
            return area;
        }

        public void AddQuery(GitHubQuery query)
        {
            _generalQueries.Add(query);
        }
    }

    internal class AreaQueries
    {
        public AreaQueries(string areaName)
        {
            AreaName = areaName;
        }

        public string AreaName { get; set; }
        public IList<GitHubQuery> Queries { get; } = new List<GitHubQuery>();
    }
}
