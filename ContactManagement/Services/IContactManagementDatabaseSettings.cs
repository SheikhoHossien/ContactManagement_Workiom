using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Services
{
    public interface IContactManagementDatabaseSettings
    {
        public string CompanyContactCollectionName{get;set;}
        public string ConnectionStrings { get;set;}
        public string DatabaseName { get;set;}
    }
}
