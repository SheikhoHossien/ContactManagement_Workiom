using ContactManagement.Models;
using ContactManagement.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactManagementRepository<Contact> contactRepository;

        public ContactController(IContactManagementRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        // GET: api/<ContactController>
        [HttpGet]
        public ActionResult<IList<Contact>> List()
        {
            var contacts = contactRepository.List();
            return contacts.ToList();
        }

        // GET api/<ContactController>/5
        [HttpGet("{id}")]
        public ActionResult<Contact> Get(int id)
        {
            var contact = contactRepository.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            return contact;
        }

        // POST api/<ContactController>
        [HttpPost]
        public ActionResult<Contact> Post([FromBody] List<JObject> contacts)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            ContactRepository contact = new ContactRepository(settings, mogoClient);
            List<Contact> contactsCollection = new List<Contact>();
            Contact FirstContactClone = contact.GetFirstRecord();
            if(FirstContactClone!=null)
            {
                if (FirstContactClone.ExtendedColumns != null)
                {
                    if (FirstContactClone.ExtendedColumns.Count > 0)
                    {
                        foreach (var key in FirstContactClone.ExtendedColumns.Keys.ToList())
                        {
                            FirstContactClone.ExtendedColumns[key] = null;
                        }


                    }
                }

            }

            foreach (JObject contactObject in contacts)
            {
                Contact obj = new Contact();
                obj.ExtendedColumns = contact.GetFirstRecord()!=null? contact.GetFirstRecord().ExtendedColumns:null;
                foreach (var contactElement in contactObject)
                {
                    //obj = new Company();

                    if (contactElement.Key == "Name")
                    {


                        obj.Name = contactElement.Value.ToString();
                    }

                    if (contactElement.Key == "Companys")
                    {
                        obj.Companys = new List<CompanyViewModel>();
                        foreach (var model in contactElement.Value.ToList())
                        {
                            CompanyViewModel View_Model = new CompanyViewModel();
                            View_Model.Name = model["Name"].ToString();
                            View_Model.NumberOfEmployees =Convert.ToInt32(model["NumberOfEmployees"]);
                            obj.Companys.Add(View_Model);
                        }
                        //obj.Companys = contactElement.Value.ToString();

                    }
                    if (FirstContactClone != null)
                    {
                        if (FirstContactClone.ExtendedColumns != null)
                        {
                            foreach (var key in FirstContactClone.ExtendedColumns.Keys.ToList())
                            {
                                if (key == contactElement.Key)
                                {
                                    obj.ExtendedColumns[key] = contactElement.Value.ToString();
                                }
                            }
                        }
                    }
                    


                }
                contactsCollection.Add(obj);
            }

                List<Contact> ContactsResult = contactRepository.Add(contactsCollection);
            if (ContactsResult == null)
            {
                return NotFound();
            }
            //return null;
            return CreatedAtAction(nameof(Get), new { id = ContactsResult }, ContactsResult);
        }
        
        [HttpPut("{id}")]
        public ActionResult<Contact> Put(int id, [FromBody] JObject new_company)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            ContactRepository contact = new ContactRepository(settings, mogoClient);
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            List<Contact> contactsCollection = new List<Contact>();
            Contact FirstContactClone = contact.GetFirstRecord();

            var existingContact = contactRepository.Find(id);
            if (existingContact == null)
            {
                return NotFound();
            }
            Contact obj = new Contact();
            obj.ExtendedColumns = existingContact.ExtendedColumns;
            foreach (var contactElement in new_company)
            {


                if (contactElement.Key == "Name")
                {


                    obj.Name = contactElement.Value.ToString();
                }

                if (contactElement.Key == "Companys")
                {
                    obj.Companys = new List<CompanyViewModel>();
                    foreach (var model in contactElement.Value.ToList())
                    {
                        CompanyViewModel View_Model = new CompanyViewModel();
                        View_Model.Name = model["Name"].ToString();
                        var com_obj=company.GetCompanyObjectByName(View_Model.Name);
                        View_Model.Id = com_obj.Id;
                        View_Model.CompanyId = com_obj.CompanyId;

                        View_Model.NumberOfEmployees = Convert.ToInt32(model["NumberOfEmployees"]);
                        obj.Companys.Add(View_Model);
                    }

                }

                if (obj.ExtendedColumns != null)
                {
                    foreach (var key in obj.ExtendedColumns.Keys.ToList())
                    {
                        if (key == contactElement.Key)
                        {
                            obj.ExtendedColumns[key] = contactElement.Value.ToString();
                        }
                    }
                }


            }



            obj.Id = existingContact.Id;
            obj.ContactId = existingContact.ContactId;

            var contact1 = contactRepository.Update(id, obj);
            if (contact1 == null)
            {
                return NotFound();
            }
            return contact1;
        }
        
        [Route("AddNewColumn")]
        [HttpPost]
        public ActionResult<Column> AddNewColumn([FromBody] Column columns)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            ContactRepository contact = new ContactRepository(settings, mogoClient);
            columns = contact.AddColumns(columns);
            if (columns == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(Get), new { id = columns }, columns);

        }
        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var contact = contactRepository.Find(id);
            if (contact == null)
            {
                return NotFound($" Contact not Found");
            }
            contactRepository.Delete(contact.ContactId);
            return Ok($" Contact Deleted Successfully");
        }

        [Route("FilterColumns")]
        [HttpPost]
        public List<Contact> FilterColumns(Dictionary<string, object> collectionFilters)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            ContactRepository contact = new ContactRepository(settings, mogoClient);
            List<Contact> contactsNames = new List<Contact>();

            contactsNames = contact.FilterColumns(collectionFilters);
            return contactsNames;
        }


    }
}
