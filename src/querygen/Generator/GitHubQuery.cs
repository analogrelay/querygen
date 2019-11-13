using System;
using System.Text.Encodings.Web;

namespace Internal.AspNetCore.QueryGenerator.Generator
{
    internal class GitHubQuery
    {
        public string Name { get; set; }
        public string QueryText { get; set; }
        public string QueryUrl { get; set; }

        internal static GitHubQuery Create(string name, string baseUrl, string queryText)
        {
            var encoded = UrlEncoder.Default.Encode(queryText);
            var queryUrl = baseUrl + encoded;
            return new GitHubQuery()
            {
                Name = name,
                QueryText = queryText,
                QueryUrl = queryUrl
            };
        }
    }
}