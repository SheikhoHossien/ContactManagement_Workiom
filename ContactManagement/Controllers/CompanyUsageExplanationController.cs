using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Controllers
{
    //We Will Explain The Difficult Situations
    
    public class CompanyUsageExplanationController : Controller
    {
        [Route("CompanyPostActionExplanation")]
        [HttpGet]
        public string CompanyPostActionExplanation()
        {
            string str =@"
        Here We Are Adding New Company      
        And You Should Insert As This Structure Like This Example =>

        [
            {
                'Name':'Asus',
                'NumberOfEmployees':10

            },
            {
                    'Name':'Toshiba',
                    'NumberOfEmployees':15

            }


        ]
        ";
                return str;
        }
        [Route("CompanyPutActionExplanation")]
        [HttpGet]
        public string CompanyPutActionExplanation()
        {
            string str = @"
            Here We Are Editing  Company      
            And You Should Insert As This Structure Like This Example =>

            {
  
              'Name': 'BlueSky',
              'NumberOfEmployees': 550,
              'ExtendedColumns': null
            }
            ";
            return str;
          
        }
        [Route("CompanyAddNewColumnActionExplanation")]
        [HttpGet]
      /*  {
  "extendedColumns": {"Address":"Mazza","Notes":"HelloWorld"}
}*/
    public string CompanyAddNewColumnActionExplanation()
        {
            string str = @"
            Here We Are Adding New Column To Extend Database Company And Column Name Should not Be Found
            in Database Column Names and also should not be any of built in Columns for example as Id or CompanyId
            And If There Any Duplicate Name with Column Name We will get Fail operation
            And Any New Company is Inserted will Inherit these inserted columns By default
            And You Edit on Them
            And You Should Insert As This Structure Like This Example =>

            {
                'extendedColumns': {'ExtendedColumnName1YouAdded':'Value','ExtendedColumnName2YouAdded':'Value'}
            }
            ";
            return str;
        }
        [Route("CompanyPostActionExplanationAfterAddColumn")]
        [HttpGet]
        public string CompanyPostActionExplanationAfterAddColumn()
        {
            string str = @"
           Here We Are Adding New Company  After Extending Schema    
           And You Should Insert As This Structure Like This Example =>
        
            [
              {
                    'Name':'AsusNEW',
                    'NumberOfEmployees':100,
                    'ExtendedColumnName1YouAdded':'AssignNewValue',
                    'ExtendedColumnName2YouAdded':'AssignNewValue',

              },
              {
                   'Name':'ToshibaNEW',
                   'NumberOfEmployees':150,
                   'ExtendedColumnName1YouAdded':'AssignNewValue',
                   'ExtendedColumnName2YouAdded':'AssignNewValue',

              }
           ]


        
        ";
            return str;
        }
        [Route("CompanyPutActionExplanationAfterAddColumn")]
        [HttpGet]
        public string CompanyPutActionExplanationAfterAddColumn()
        {
            string str = @"
           Here We Are Editig  Company  After Extending Schema    
           And You Should Insert As This Structure Like This Example =>
        
            {
                 'Name':'Toshiba_NEW_CALM',
                 'NumberOfEmployees':1500,
                 'ExtendedColumnName1YouAdded':'EditedValue',
                 'ExtendedColumnName2YouAdded':'EditedValue',

            }


        
        ";
            return str;

        }
        [Route("CompanyFilterColumnsActionExplanation")]
        [HttpGet]
        public string CompanyFilterColumnsActionExplanation()
        {
            string str = @"
            Here We Are Filtering On  Company Details     
            And You Should Insert As This Structure Like This Example =>
            

            { 'Name': 'Asus','ExtendedColumnNameYouAdded': 'Value'}
            ";
            return str;
            
        }
    }
}
