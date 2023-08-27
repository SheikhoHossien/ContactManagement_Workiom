using ContactManagement.Exceptions;
using ContactManagement.Models;
using ContactManagement.Validators;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Services
{
    public class ContactRepository : IContactManagementRepository<Contact>
    {
        private readonly IMongoCollection<Contact> _contacts;
        
        public ContactRepository(IContactManagementDatabaseSettings settings, IMongoClient mongoClient)
        {
            settings.DatabaseName = "contactmanagement";
            settings.CompanyContactCollectionName = "contact";
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _contacts = database.GetCollection<Contact>(settings.CompanyContactCollectionName);

        }
        public List<Contact> FilterColumns(Dictionary<string, object> collectionFilters)
        {
            List<Contact> contacts = new List<Contact>();
            List<Contact> subcontacts = new List<Contact>();
            List < string> elements = new List<string>();
            Contact FirstContactClone = GetFirstRecord();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            if (!collectionFilters.ContainsKey("Name"))
                {
                    collectionFilters.Add("Name", "");
                }
                
                if (!collectionFilters.ContainsKey("ContactId"))
                {
                    collectionFilters.Add("ContactId", 0);
                }
                if (!collectionFilters.ContainsKey("Companys"))
                {
                    collectionFilters.Add("Companys",new JObject());
                }
                if (collectionFilters.ContainsKey("Companys"))
                {
                    foreach (var ValParent in collectionFilters)
                    {
                        if (ValParent.Key == "Companys")
                        {
                            JObject jObj = ValParent.Value is JObject ? (JObject)ValParent.Value : JObject.FromObject(ValParent.Value);
                            foreach (var d in jObj)
                            {
                                if (d.Key == "Name")
                                {
                                    elements.Add(d.Value.ToString());

                                }

                            }

                        }


                    }
                }
                if(FirstContactClone!=null)
                {
                    if (FirstContactClone.ExtendedColumns != null)
                    {
                        if (FirstContactClone.ExtendedColumns.Count > 0)
                        {
                            foreach (var key2 in FirstContactClone.ExtendedColumns.Keys.ToList())
                            {
                                if (collectionFilters.ContainsKey(key2))
                                {
                                    keyValuePairs.Add(key2, collectionFilters[key2]);
                                }


                            }


                        }
                    }
                }
                
                if(keyValuePairs.Count==0&&collectionFilters["Name"].ToString() == ""&& keyValuePairs.Count == 0&& elements.Count == 0&& Convert.ToInt32(collectionFilters["ContactId"]) == 0)
                {
                    return null;

                }
                string val;
                 _contacts.Find(contact => true).
                                        ToList().Where<Contact>(x =>
                (collectionFilters["Name"].ToString() == "" || (collectionFilters["Name"].ToString() != "" && collectionFilters["Name"].ToString() == x.Name)) &&
                (Convert.ToInt32(collectionFilters["ContactId"]) == 0 || (Convert.ToInt32(collectionFilters["ContactId"]) != 0 && Convert.ToInt32(collectionFilters["ContactId"]) == x.ContactId)) &&

                (elements.Count == 0 || (elements.Count > 0 && elements.TrueForAll(z => x.Companys.Select(y => y.Name).Contains(z))))


                ).ToList().ForEach(v=>
                {
                    if(keyValuePairs.Count > 0)
                    {
                        foreach (var element1 in keyValuePairs)
                        {
                            if (v.ExtendedColumns.Count > 0)
                            {
                                foreach (var element2 in v.ExtendedColumns)
                                {
                                    if(element1.Key==element2.Key&& element1.Value.ToString()==element2.Value.ToString())
                                    {
                                        subcontacts.Add(v);
                                    }

                                }
                            }

                        }
                    }
                    if (keyValuePairs.Count == 0)
                    {
                        subcontacts.Add(v);
                    }



                });


                return subcontacts;
            
            
        }
       

        public IList<Contact> List()
        {
            var contacts = _contacts.Find(contact => true).ToList();
            return contacts;
        }
        public Contact GetCompanyObjectByName(string Name)
        {
            if (Name == null)
            {
                ContactRequestValidator.NotFoundContactElement("There Is No Name Value");
            }
            return _contacts.Find(contact => contact.Name == Name).FirstOrDefault();
        }
        public Contact CheckUniqueColumnName(string Name)
        {
            if (Name == null)
            {
                ContactRequestValidator.NotFoundContactElement("There Is No Name Value");
            }

            return _contacts.Find(contact => contact.ExtendedColumns.ContainsKey(Name)).FirstOrDefault();
        }
        public Contact CheckUniqueNameAdd(string Name)
        {
            if (Name == null)
            {
                ContactRequestValidator.NotFoundContactElement("There Is No Name Value");
            }
            return _contacts.Find(contact => contact.Name == Name).FirstOrDefault();
        }
        public Company GetCompanyObjectByCompanyName(string Name)
        {
            if (Name == null)
            {
                ContactRequestValidator.NotFoundContactElement("There Is No Name Value");
            }
            MongoClient mogoClient=new MongoClient();
            ContactManagementDatabaseSettings settings=new ContactManagementDatabaseSettings();
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            Company com=company.GetCompanyObjectByName(Name);
            return com;
            //return company.Find(company => company.Name == Name).FirstOrDefault();
        }
        public Contact GetFirstRecord()
        {
            return _contacts.AsQueryable().FirstOrDefault();
        }
        public List<Contact> Add(List<Contact> contacts)
        {
            
            if (contacts.Count > 0)
            {
                foreach (Contact ValidElement in contacts)
                {
                    ContactRequestValidator.Validate(ValidElement);
                }
                int LastContactId = 0;
                var LastInsertedContact = this._contacts.Find(new BsonDocument()).Sort(new BsonDocument("ContactId", -1)).FirstOrDefault();
                if (LastInsertedContact != null)
                {
                    LastContactId = LastInsertedContact.ContactId;

                }
                else
                {
                    LastContactId = 0;
                }
                foreach (Contact contact in contacts)
                {
                    LastContactId++;
                    if (String.IsNullOrEmpty(contact.Name.ToString()))
                    {
                        return null;
                    }
                    var companyfilter = CheckUniqueNameAdd(contact.Name);
                    if (companyfilter == null)
                    {



                        //int LastCompanyId = _companys.Find(new BsonDocument()).Sort(new BsonDocument("$CompanyId", -1)).FirstOrDefaultAsync()
                        contact.ContactId = LastContactId;
                        if (contact.Companys != null && contact.Companys.Count > 0)
                        {
                            foreach (CompanyViewModel element in contact.Companys)
                            {
                                var result = GetCompanyObjectByCompanyName(element.Name);
                                if (result == null)
                                {
                                    //return null;
                                    ContactRequestValidator.NotFoundContactElement($"There Is No Company {element.Name}");
                                }
                                else
                                {
                                    element.Id = result.Id;
                                    element.CompanyId = result.CompanyId;
                                    element.Name = result.Name;
                                    element.NumberOfEmployees = result.NumberOfEmployees;
                                }
                            }
                        }
                        //return company;

                    }
                    else
                    {
                        ContactRequestValidator.DuplicatedName(contact.Name);
                    }

                }
                _contacts.InsertMany(contacts);
                return contacts;


            }
            else
            {
                ContactRequestValidator.DuplicatedName("There is No Contact Elements");
                return null;
            }
                
                
            
        }
        public Contact Find(int id)
        {
            int number;

            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new ContactNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new ContactNotFoundException("Id Has Not Integer Value");
            }
            Contact cont = _contacts.Find(contact => contact.ContactId == id).FirstOrDefault();

            if (cont != null)
            {
                return cont;
            }
            else
            {

                throw new ContactNotFoundException("Contact Data Not Found");

            }
        }
        public List<Contact> Search(string term)
        {
            return _contacts.Find(contact => true).ToList();
        }


        public Contact Get(int id)
        {
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new ContactNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new ContactNotFoundException("Id Has Not Integer Value");
            }
            Contact contact= _contacts.Find(contact => contact.ContactId == id).FirstOrDefault();
            if (contact != null)
            {
                return contact;
            }
            else
            {

                throw new ContactNotFoundException("Contact Data Not Found");
            }
        }

        public void Delete(int id)
        {
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new ContactNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new ContactNotFoundException("Id Has Not Integer Value");
            }
            Find(id);
            _contacts.DeleteOne(contact => contact.ContactId == id);
        }
        public Contact CheckUniqueNameUpdate(int id,string Name)
        {
            int number;
            if (int.TryParse(id.ToString(), out number) && id == 0)
            {
                throw new ContactNotFoundException("Please Enter Valid Value");
            }
            if (!int.TryParse(id.ToString(), out number))
            {
                throw new ContactNotFoundException("Id Has Not Integer Value");
            }
            if (Name == null)
            {
                throw new ContactNotFoundException("Please Enter Name Values");
            }
            var oldContact= _contacts.Find(contact => contact.ContactId == id).FirstOrDefault();

            return _contacts.Find(contact => contact.Name == Name && Name!=oldContact.Name).FirstOrDefault();
        }
        public Contact Update(int id, Contact contact)
        {
            ContactRequestValidator.Validate(contact);
            if (String.IsNullOrEmpty(contact.Name.ToString())|| String.IsNullOrEmpty(id.ToString()))
            {
                return null;
            }
            Find(id);
            var contactfilter = CheckUniqueNameUpdate(id,contact.Name);
            if (contactfilter == null)
            {
                _contacts.ReplaceOne(contact => contact.ContactId == id, contact);
                return contact;
            }
            else
            {
                ContactRequestValidator.DuplicatedName(contact.Name);
                return null;
            }
            
        }

        public Column AddColumns(Column columns)
        {


            if (columns.ExtendedColumns.Count > 0)
            {

                foreach (var column in columns.ExtendedColumns)
                {

                    if (column.Key == "Name")
                    {
                        ContactRequestValidator.NotFoundContactElement("There is Duplicated Column Name with Name ");
                    }
                    if (column.Key == "ContactId")
                    {
                        ContactRequestValidator.NotFoundContactElement("There is Duplicated Column Name with ContactId ");
                    }
                    if (column.Key == "Id")
                    {
                        ContactRequestValidator.NotFoundContactElement("There is Duplicated Column Name with Id ");
                    }
                    if (column.Key == "Companys")
                    {
                        ContactRequestValidator.NotFoundContactElement("There is Duplicated Column Name with Companys ");
                    }
                    Contact obj = CheckUniqueColumnName(column.Key);
                    if (obj != null)
                    {
                        return null;

                    }

                }


                var update = Builders<Contact>.Update.Set("ExtendedColumns", columns.ExtendedColumns);
                var filter = Builders<Contact>.Filter.Empty;
                var options = new UpdateOptions { IsUpsert = true };

                _contacts.UpdateMany(filter, update, options);
                return columns;
            }
            else
            {
                return null;
            }



        }
    }
}
