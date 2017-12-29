using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cosmosapp.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;

namespace cosmosapp.Repositories
{
    public class GourmetRepository : RepositoryBase<Gourmet>
    {
        public GourmetRepository(DocumentClient documentClient)
            : base(documentClient, "Gourmet", "GourmetCollection")
        {
        }

        public async Task<IEnumerable<Gourmet>> GetByDistanceAsync(double lang, double lat, int distance)
        {
            var documentQuery = Client.CreateDocumentQuery<Gourmet>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                                        .Where(x => x.Location.Distance(new Point(lang, lat)) < distance)
                                        //.OrderBy(x => x.Location)
                                        .AsDocumentQuery();

            var results = new List<Gourmet>();
            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<Gourmet>());
            }

            return results;
        }
        public async Task<IEnumerable<Gourmet>> GetAllAsync()
        {
            var documentQuery = Client.CreateDocumentQuery<Gourmet>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                                        //.Where(x => x.Season == 3)
                                        .OrderBy(x => x.Episode)
                                        .AsDocumentQuery();

            var results = new List<Gourmet>();
            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<Gourmet>());
            }

            return results;
        }

    }
}