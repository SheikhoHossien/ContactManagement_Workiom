using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContactManagement.Models
{
    public class Contact
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [JsonIgnore]
        [BsonElement("ContactId")]
        public int ContactId { get; set; }
        [Required]
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Companys")]
        public List<CompanyViewModel> Companys { get; set; }
        [BsonElement("[]")]
        public Dictionary<string, object> ExtendedColumns { get; set; }


    }
}
