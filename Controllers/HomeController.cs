using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cosmosapp.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Net;
using cosmosapp.Repositories;

namespace cosmosapp.Controllers
{
    public class HomeController : Controller
    {
        private const string EndpointUri = "https://xxx1216.documents.azure.com:443/";
        private const string PrimaryKey = "dG4VVoVXPVWIj1x67SV3m6ntBgPwxxxMP459eqNoqaLgSFI3c4VFfp8w4fwK2kc3OxOd36gYKmoxYrnaV2YE5w==";
//        private DocumentClient client;
        private readonly PersonRepository _repository;

        public HomeController(PersonRepository repository)
        {
            _repository = repository;
        }


        
        public async Task<IActionResult> Index()
        {
            var person = await _repository.GetByNameAsync("クトリ");
            if (person == null)
            {
                person = new Person{
                    Name = "クトリ"
                };
                await _repository.CreateAsync(person);
                person = await _repository.GetAsync(person.Id);
            }
            person.Name = "ネフレン";
            await _repository.UpdateAsync(person);

            var test = await _repository.GetAllAsync("ネフレン");

            await _repository.DeleteAsync(person);


//             var databaseName = "Person";
//             var collectionName = "PersonCollection";
//             this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
//             await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
//             await this.client.CreateDocumentCollectionIfNotExistsAsync(
//                 UriFactory.CreateDatabaseUri(databaseName),
//                 new DocumentCollection { Id = collectionName });

//             var person = new Person{
// //                Id = "1",
//                 Name = "クトリ"
//             };

//             try
//             {
//                 await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, /*person.Id*/"1"));
//             }
//             catch (DocumentClientException de)
//             {
//                 if (de.StatusCode == HttpStatusCode.NotFound)
//                 {
//                     await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), person);
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }

//             FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
//             IQueryable<Person> personQuery = this.client.CreateDocumentQuery<Person>(
//                 UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
//                 .Where(f => f.Name == "クトリ");

//             // foreach (Person p in personQuery)
//             // {
//             //     var test = p;
//             //     test.Name = "ネフレン";
//             //     await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, test.Id), test);
//             //     await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, test.Id));
//             // }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
