

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Spatial;
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
    }

}