using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Internal.AspNetCore.QueryGenerator.Model
{
    internal class QueryArea
    {
        public string Name { get; set; }
        public string Label { get; set; }

        [JsonPropertyName("repos")]
        public IList<string> Repositories { get; set; } = new List<string>() { "dotnet/aspnetcore" };

        [JsonPropertyName("queries")]
        public IList<string> Queries { get; set; } = new List<string>();
    }
}