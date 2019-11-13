using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Internal.AspNetCore.QueryGenerator.Generator;
using Internal.AspNetCore.QueryGenerator.Model;
using Stubble.Core.Builders;

namespace Internal.AspNetCore.QueryGenerator
{
    class Program
    {
        // Through the magic of System.CommandLine.Dragonfruit, this method is turned into a command-line API!
        // The XML docs become help text and the arguments become switches/args.
        /// <summary>
        /// Generates GitHub query lists from an input JSON file.
        /// </summary>
        /// <param name="inputFile">The JSON data file to use as input. Defaults to 'queries.json' in the current directory.</param>
        /// <param name="outputFile">The output markdown file to generate. Defaults to 'Queries.md' in the current directory.</param>
        static async Task<int> Main(string inputFile = null, string outputFile = null)
        {
            inputFile = inputFile ?? Path.Combine(Directory.GetCurrentDirectory(), "queries.json");
            outputFile = outputFile ?? Path.Combine(Directory.GetCurrentDirectory(), "Queries.md");

            if(!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Could not find input file: {inputFile}");
                return 1;
            }

            // Load the input file
            Console.WriteLine($"Loading input data: {inputFile} ...");
            QueryList inputList;
            using(var inputStream = File.OpenRead(inputFile))
            {
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                };
                options.Converters.Add(new JsonStringEnumConverter());
                inputList = await JsonSerializer.DeserializeAsync<QueryList>(inputStream, options);
            }

            // Generate queries
            Console.WriteLine("Computing queries...");
            var querySet = new QuerySet();
            foreach(var queryDefinition in inputList.Queries)
            {
                GenerateQuery(queryDefinition, inputList, querySet);
            }

            // Generate the template
            Console.WriteLine($"Generating output file: {outputFile} ...");
            var stubble = new StubbleBuilder().Build();
            using(var templateStream = typeof(Program).Assembly.GetManifestResourceStream("Internal.AspNetCore.QueryGenerator.Templates.querylist.mustache"))
            using(var templateReader = new StreamReader(templateStream))
            {
                var template = await templateReader.ReadToEndAsync();
                var output = await stubble.RenderAsync(template, querySet);
                await File.WriteAllTextAsync(outputFile, output);
            }

            return 0;
        }

        private static void GenerateQuery(QueryDefinition queryDefinition, QueryList inputList, QuerySet querySet)
        {
            if(queryDefinition.Grouping == QueryGrouping.Area)
            {
                foreach(var area in inputList.Areas)
                {
                    GenerateAreaQuery(queryDefinition, area, querySet);
                }
            }
            else
            {
                throw new NotSupportedException("Only area-grouped queries are supported at this time.");
            }
        }

        private static void GenerateAreaQuery(QueryDefinition queryDefinition, QueryArea area, QuerySet querySet)
        {
            string baseUrl;
            string areaFilter;

            if(area.Repositories.Count > 1)
            {
                baseUrl = "https://github.com/search?q=";
                areaFilter = string.Join(" ", area.Repositories.Select(r => $"repo:{r}")) + " ";
            }
            else
            {
                baseUrl = $"https://github.com/{area.Repositories[0]}/issues?q=";
                areaFilter = "";
            }

            areaFilter += $"label:{area.Label}";

            var query = GitHubQuery.Create(queryDefinition.Name, baseUrl, $"{queryDefinition.QueryText} {areaFilter}");
            querySet.AddAreaQuery(area.Name, query);
        }
    }
}
