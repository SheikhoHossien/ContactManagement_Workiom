using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContactManagement.Models
{
    public class CompanyViewModel
    {
        //[System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        //  [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        [BsonElement("CompanyId")]

        public int CompanyId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [JsonIgnore]

        [BsonElement("NumberOfEmployees")]
        public int NumberOfEmployees { get; set; }
      


    }
}
