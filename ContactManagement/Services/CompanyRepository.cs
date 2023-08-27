using ContactManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using ContactManagement.Validators;
using ContactManagement.Exceptions;

namespace ContactManagement.Services
{
    public class CompanyRepository  : IContactManagementRepository<Company>
    {
        private readonly IMongoCollection<Company> _companys;

        public CompanyRepository(IContactManagementDatabaseSettings settings,IMongoClient mongoClient)
        {
            settings.DatabaseName = "contactmanagement";
            settings.CompanyContactCollectionName = "company";
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _companys = database.GetCollection<Company>(settings.CompanyContactCollectionName);

        }
        public List<Company> FilterColumns(Dictionary<string, object> collectionFilters)
        {
            List<Company> companys = new List<Company>();
            List<Company> subcompanys = new List<Company>();
            List<string> elements = new List<string>();
            Company FirstCompanyClone = GetFirstRecord();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            if (!collectionFilters.ContainsKey("Name"))
            {
                collectionFilters.Add("Name", "");
            }

            if (!collectionFilters.ContainsKey("CompanyId"))
            {
                collectionFilters.Add("CompanyId", 0);
            }
            if(FirstCompanyClone!=null)
            {
                if (FirstCompanyClone.ExtendedColumns != null)
                {
                    if (FirstCompanyClone.ExtendedColumns.Count > 0)
                    {
                        foreach (var key2 in FirstCompanyClone.ExtendedColumns.Keys.ToList())
                        {
                            if (collectionFilters.ContainsKey(key2))
                            {
                                keyValuePairs.Add(key2, collectionFilters[key2]);
                            }


                        }


                    }
                }
            }
            
            if (keyValuePairs.Count == 0 && collectionFilters["Name"].ToString() == "" && keyValuePairs.Count == 0  && Convert.ToInt32(collectionFilters["CompanyId"]) == 0)
            {
                return null;

            }
            string val;
            _companys.Find(contact => true).
                                   ToList().Where<Company>(x =>
           (collectionFilters["Name"].ToString() == "" || (collectionFilters["Name"].ToString() != "" && collectionFilters["Name"].ToString() == x.Name)) &&
           (Convert.ToInt32(collectionFilters["CompanyId"]) == 0 || (Convert.ToInt32(collectionFilters["CompanyId"]) != 0 && Convert.ToInt32(collectionFilters["CompanyId"]) == x.CompanyId)) 

          

           ).ToList().ForEach(v =>
           {
               if (keyValuePairs.Count > 0)
               {
                   foreach (var element1 in keyValuePairs)
                   {
                       if (v.ExtendedColumns.Count > 0)
                       {
                           foreach (var element2 in v.ExtendedColumns)
                           {
                               if (element1.Key == element2.Key && element1.Value.ToString() == element2.Value.ToString())
                               {
                                   subcompanys.Add(v);
                               }

                           }
                       }

                   }
               }
               if (keyValuePairs.Count == 0)
               {
                   subcompanys.Add(v);
               }



           });


            return subcompanys;


        }

        public IList<Company> List()
        {

            var companys= _companys.Find(company => true).ToList();
            return companys;
            
        }
        public Company GetCompanyObjectByName(string Name)
        {
            if(Name==null)
            {
                CompanyRequestValidator.NotFoundCompanyElement("There Is No Name Value");
            }
            
            Company com= _companys.Find(company => company.Name == Name).FirstOrDefault();
            return com;
        }
        public Company CheckUniqueNameAdd(string Name)
        {
            if (Name == null)
            {
                CompanyRequestValidator.NotFoundCompanyElement("There Is No Name Value");
            }
            return _companys.Find(company => company.Name == Name ).FirstOrDefault();
        }
        public Company CheckUniqueColumnName(string Name)
        {
            if (Name == null)
            {
                CompanyRequestValidator.NotFoundCompanyElement("There Is No Name Value");
            }
            return _companys.Find(company =>  company.ExtendedColumns.ContainsKey(Name)).FirstOrDefault();
        }
        public Column AddColumns(Column columns)
        {
            
            
            if (columns.ExtendedColumns.Count > 0)
            {
                
                foreach (var column in columns.ExtendedColumns)
                {
                    
                    if(column.Key=="Name")
                    {
                        CompanyRequestValidator.NotFoundCompanyElement("There is Duplicated Column Name with Name ");
                    }
                    if (column.Key == "CompanyId")
                    {
                        CompanyRequestValidator.NotFoundCompanyElement("There is Duplicated Column Name with CompanyId ");
                    }
                    if (column.Key == "Id")
                    {
                        CompanyRequestValidator.NotFoundCompanyElement("There is Duplicated Column Name with Id ");
                    }
                    if (column.Key == "NumberOfEmployees")
                    {
                        CompanyRequestValidator.NotFoundCompanyElement("There is Duplicated Column Name with NumberOfEmployees ");
                    }
                    Company obj=CheckUniqueColumnName(column.Key);
                    if(obj!=null)
                    {
                        return null;

                    }

                }

                
                var update = Builders<Company>.Update.Set("ExtendedColumns", columns.ExtendedColumns);
                var filter = Builders<Company>.Filter.Empty;
                var options = new UpdateOptions { IsUpsert = true };

                _companys.UpdateMany(filter, update, options);
                return columns;
            }
            else
            {
                return null;
            }
            


        }
        public Company GetFirstRecord()
        {
            Company com= _companys.AsQueryable().FirstOrDefault();
            return com;
        }
        public List<Company> Add (List<Company> companys)
        {
            
            if (companys.Count>0)
            {
                foreach (Company ValidElement in companys)
                {
                    CompanyRequestValidator.Validate(ValidElement);
                }
                int LastCompanyId = 0;
                var LastInsertedCompany = this._companys.Find(new BsonDocument()).Sort(new BsonDocument("CompanyId", -1)).FirstOrDefault();
                if (LastInsertedCompany != null)
                {
                    LastCompanyId = LastInsertedCompany.CompanyId ;

                }
                else
                {
                    LastCompanyId = 0;
                }
                foreach (Company company in companys)
                {
                    LastCompanyId++;
                    if (String.IsNullOrEmpty(company.Name.ToString()))
                    {
                        return null;
                    }
                    var companyfilter = CheckUniqueNameAdd(company.Name);
                    if (companyfilter == null)
                    {

                        company.CompanyId = LastCompanyId;
                        

                    }
                    else
                    {
                        CompanyRequestValidator.DuplicatedName(company.Name);
                        //return Ok($" Company Deleted Successfully");
                    }
                    
                }
                _companys.InsertMany(companys);
                return companys;


            }
            else
            {
                CompanyRequestValidator.DuplicatedName("There is No Company Elements");
                return null;
            }
            
            
        }
        
        public Company Find(int id)
        {
            int number;
            
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new CompanyNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new CompanyNotFoundException("Id Has Not Integer Value");
            }
            Company comp = _companys.Find(company => company.CompanyId == id).FirstOrDefault();
            if (comp != null)
            {
                return comp;
            }
            else
            {

                throw new CompanyNotFoundException("Company Data Not Found");
                    
            }
            
            
             
            







        }
        public List<Company> Search(string term)
        {

            return _companys.Find(company=>true).ToList();
        }
        

        public Company Get(int id)
        {
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new CompanyNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new CompanyNotFoundException("Id Has Not Integer Value");
            }
            Company comp= _companys.Find(company => company.CompanyId==id).FirstOrDefault();
            if (comp != null)
            {
                return comp;
            }
            else
            {

                throw new CompanyNotFoundException("Company Data Not Found");
            }
        }

        public void Delete(int id)
        {
            
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new CompanyNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new CompanyNotFoundException("Id Has Not Integer Value");
            }
            Find(id);
            _companys.DeleteOne(company => company.CompanyId == id);
        }
        public Company CheckUniqueNameUpdate(int id, string Name)
        {
            
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new CompanyNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new CompanyNotFoundException("Id Has Not Integer Value");
            }
            if(Name==null)
            {
                throw new CompanyNotFoundException("Please Enter Name Values");
            }
            var oldCompany = _companys.Find(company => company.CompanyId == id).FirstOrDefault();

            return _companys.Find(company => company.Name == Name && Name != oldCompany.Name).FirstOrDefault();
        }
        public Company Update(int id, Company company)
        {
            CompanyRequestValidator.Validate(company);
            
            if (String.IsNullOrEmpty(company.Name.ToString()) || String.IsNullOrEmpty(id.ToString()))
            {
                return null;
            }
            Find(id);
            
            var companyfilter = CheckUniqueNameUpdate(id, company.Name);
            if (companyfilter == null)
            {
                _companys.ReplaceOne(company => company.CompanyId == id, company);
                return company;
            }
            else
            {
                CompanyRequestValidator.DuplicatedName(company.Name);
                return null;
            }
            
        }

        
    }
}
