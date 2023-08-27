using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Services
{
    public class ContactManagementDatabaseSettings:IContactManagementDatabaseSettings
    {
        public string CompanyContactCollectionName { get; set; } = "company";
        public string ConnectionStrings { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "contactmanagement";
    }
}
