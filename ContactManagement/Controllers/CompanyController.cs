using ContactManagement.Models;
using ContactManagement.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IContactManagementRepository<Company> companyRepository;

        public CompanyController(IContactManagementRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        
        // GET: api/<CompanyController>
        [HttpGet]
        public ActionResult<IList<Company>> List()
        {
            var companys= companyRepository.List();
            return companys.ToList();
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        public ActionResult<Company> Get(int id)
        {
            var company = companyRepository.Find(id);
            if(company==null)
            {
                return NotFound();
            }
            return company;
        }

        
        // POST api/<CompanyController>
        [HttpPost] 
        
        public ActionResult<Company> Post([FromBody] List<JObject> companys)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            List<Company> companysCollection = new List<Company>();
            Company FirstCompanyClone = company.GetFirstRecord();
            if(FirstCompanyClone!=null)
            {
                if (FirstCompanyClone.ExtendedColumns != null)
                {
                    if (FirstCompanyClone.ExtendedColumns.Count > 0)
                    {
                        foreach (var key in FirstCompanyClone.ExtendedColumns.Keys.ToList())
                        {
                            FirstCompanyClone.ExtendedColumns[key] = null;
                        }


                    }
                }
            }
            
            
            foreach (JObject companyObject in companys)
            {
                Company obj = new Company();
                obj.ExtendedColumns = company.GetFirstRecord() != null ? company.GetFirstRecord().ExtendedColumns : null; ;
                foreach (var companyElement in companyObject)
                {
                     //obj = new Company();

                    if (companyElement.Key == "Name")
                    {


                        obj.Name = companyElement.Value.ToString();
                    }

                    if (companyElement.Key == "NumberOfEmployees")
                    {
                        int num;
                        bool success = int.TryParse(companyElement.Value.ToString(), out num);
                        if (success)
                        {
                            obj.NumberOfEmployees = Convert.ToInt32(companyElement.Value);
                        }


                    }
                    //obj.ExtendedColumns = company.GetFirstRecord().ExtendedColumns;
                    if(FirstCompanyClone!=null)
                    {
                        if (FirstCompanyClone.ExtendedColumns != null)
                        {
                            foreach (var key in FirstCompanyClone.ExtendedColumns.Keys.ToList())
                            {
                                if (key == companyElement.Key)
                                {
                                    obj.ExtendedColumns[key] = companyElement.Value.ToString();
                                }
                            }
                        }
                    }
                    
                    
                    
                }
                companysCollection.Add(obj);
            }

            List<Company>CompanysResult = companyRepository.Add(companysCollection);
            if (CompanysResult == null)
            {
                return NotFound();
            }
            //return null;
            return CreatedAtAction(nameof(Get), new { id = CompanysResult }, CompanysResult);
        }

        


        // PUT api/<CompanyController>/5
        [HttpPut("{id}")]
        public ActionResult<Company> Put(int id, [FromBody] JObject new_company)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            List<Company> companysCollection = new List<Company>();
            var existingCompany = companyRepository.Find(id);
            if(existingCompany==null)
            {
                return NotFound();
            }
            Company obj = new Company();
            obj.ExtendedColumns = existingCompany.ExtendedColumns;
            foreach (var companyElement in new_company)
            {


                if (companyElement.Key == "Name")
                {


                    obj.Name = companyElement.Value.ToString();
                }

                if (companyElement.Key == "NumberOfEmployees")
                {
                    int num;
                    bool success = int.TryParse(companyElement.Value.ToString(), out num);
                    if (success)
                    {
                        obj.NumberOfEmployees = Convert.ToInt32(companyElement.Value);
                    }
                    else
                    {
                        return null;
                    }


                }
                
                if (existingCompany.ExtendedColumns != null)
                {
                    foreach (var key in existingCompany.ExtendedColumns.Keys.ToList())
                    {
                        if (key == companyElement.Key)
                        {
                            obj.ExtendedColumns[key] = companyElement.Value.ToString();
                        }
                    }
                }


            }



            obj.Id = existingCompany.Id;
            obj.CompanyId = existingCompany.CompanyId;

            var company1 = companyRepository.Update(id, obj);
            if (company1 == null)
            {
                return NotFound();
            }
            return company1;
            //return Ok("Updated Successfully");
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var company = companyRepository.Find(id);
            if(company==null)
            {
                return NotFound($" Company not Found");
            }
            companyRepository.Delete(company.CompanyId);
            return Ok($" Company Deleted Successfully");
        }
        [Route("AddNewColumn")]
        [HttpPost]
        public ActionResult<Column> AddNewColumn([FromBody] Column column)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            column = company.AddColumns(column);
            if (column == null)
            {
                return NotFound();
            }
            //return;
            return CreatedAtAction(nameof(Get), new { id = column }, column);

        }
        [Route("FilterColumns")]
        [HttpPost]
        public List<Company> FilterColumns(Dictionary<string, object> collectionFilters)
        {
            MongoClient mogoClient = new MongoClient();
            ContactManagementDatabaseSettings settings = new ContactManagementDatabaseSettings();
            CompanyRepository company = new CompanyRepository(settings, mogoClient);
            List<Company> companysNames = new List<Company>();

            companysNames = company.FilterColumns(collectionFilters);
            return companysNames;
        }
    }
}
