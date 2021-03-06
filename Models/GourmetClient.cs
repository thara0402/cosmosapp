

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

namespace cosmosapp.Models
{
    public static class GourmetClient
    {
        public static List<Gourmet> Create(string path)
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var result = JsonConvert.DeserializeObject<List<Gourmet>>(json);
            foreach (var gourmet in result)
            {
                gourmet.Location = new Point(gourmet.Location2.Lng, gourmet.Location2.Lat);
            }
            return result;
        }

        public static async Task Search(double lng, double lat)
        {
            var azureSearchName = "xxx1216";
            var azureSearchKey = "287E8CBE90CBC82392413E24384C3A9F";
            var indexName = "gourmet-index";

            var searchClient = new SearchServiceClient(azureSearchName, new SearchCredentials(azureSearchKey));
            var indexClient = searchClient.Indexes.GetClient(indexName);

            var geo = string.Format("geo.distance(location, geography'POINT({0} {1})')", lng, lat);
            var parameters = new SearchParameters()
            {
                //Select = new[] { "title" },
                //Filter = "season eq 2",
                //OrderBy = new[] { "season asc", "episode asc" },
                OrderBy = new[] { geo },
                
                Top = 10
            };
            // https://docs.microsoft.com/en-us/rest/api/searchservice/OData-Expression-Syntax-for-Azure-Search

            var gourmetList = new List<Gourmet>();
            var results = await indexClient.Documents.SearchAsync<Gourmet>("*", parameters);
            foreach (var gourment in results.Results)
            {
                var title = gourment.Document.Title;
                gourmetList.Add(gourment.Document);
            }
            
            var continuationToken = results.ContinuationToken;
            while (continuationToken != null)
            {
                // Skip 50 にしか対応していない
                if (parameters.Skip == null)
                {
                    parameters.Skip = 0;
                }
                parameters.Skip += 50;

                var temp = await indexClient.Documents.SearchAsync<Gourmet>("*", parameters);
                foreach (var gourment in temp.Results)
                {
                    var title = gourment.Document.Title;
                    gourmetList.Add(gourment.Document);
                }
                continuationToken = temp.ContinuationToken;
            }
        }
    }

}