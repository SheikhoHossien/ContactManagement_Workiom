using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContactManagement.Models
{

    public class Company
    {
        //[System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        //  [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        [BsonElement("CompanyId")]

        public int CompanyId { get; set; }
        [Required]
        //[Index(nameof(Name), IsUnique = true)]
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("NumberOfEmployees")]
        public int NumberOfEmployees { get; set; }
        //public BsonArray ExtendedColumns { get; set; }
        [BsonElement("[]")]
        public Dictionary<string, object> ExtendedColumns { get; set; }

    }       
}
