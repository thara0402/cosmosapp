using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cosmosapp.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace cosmosapp.Repositories
{
    public class PersonRepository : RepositoryBase<Person>
    {
        public PersonRepository(DocumentClient documentClient)
            : base(documentClient, "Person", "PersonCollection")
        {
        }

        public async Task<Person> GetByNameAsync(string name)
        {
            var documentQuery = Client.CreateDocumentQuery<Person>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                                        .Where(x => x.Name == name)
                                        .AsDocumentQuery();

            return (await documentQuery.ExecuteNextAsync<Person>()).FirstOrDefault();
        }
        public async Task<IEnumerable<Person>> GetAllAsync(string name)
        {
            var documentQuery = Client.CreateDocumentQuery<Person>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                                        .Where(x => x.Name == name)
                                        .AsDocumentQuery();

            var results = new List<Person>();
            while (documentQuery.HasMoreResults)
            {
                results.AddRange(await documentQuery.ExecuteNextAsync<Person>());
            }

            return results;
        }

    }
}