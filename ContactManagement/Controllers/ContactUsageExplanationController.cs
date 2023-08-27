using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Controllers
{
    public class ContactUsageExplanationController : Controller
    {
        [Route("ContactPostActionExplanation")]
        [HttpGet]
        public string ContactPostActionExplanation()
        {
            string str = @"
        Here We Are Adding New Contact and we should add Company Found in Company Database     
        And You Should Insert As This Structure Like This Example =>

        [
          {
            'Name':'Sheikho',
            'Companys':
             [
                 {
                   'Name':'Asus',
                   'NumberOfEmployees':10
                 },
                 {
                   'Name':'Toshiba',
                   'NumberOfEmployees':16
                 }

             ]
          },
          {
            'Name':'Hayan',
            'Companys':
             [
                 { 
                   'Name':'Asus',
                   'NumberOfEmployees':18
                 },
                 { 
                   'Name':'Toshiba',
                   'NumberOfEmployees':19
                 }
             ]
          }
        ]
        ";
            return str;
        }
        [Route("ContactPutActionExplanation")]
        [HttpGet]
        public string ContactPutActionExplanation()
        {
            string str = @"
            Here We Are Editing  Contact      
            And You Should Insert As This Structure Like This Example =>

            {
    
                'Name': 'Hayan11NEW',
                'Companys': [
                  {

                    'Name': 'Toshiba_NEW_CALM',
                    'NumberOfEmployees': 1000
                  },
                  {

                    'Name': 'AsusNEW',
                    'NumberOfEmployees': 1500
                  }
                ],
                'ExtendedColumns': null
             }
        
            ";
            return str;
        }
        [Route("ContactAddNewColumnActionExplanation")]
        [HttpGet]
        public string ContactAddNewColumnActionExplanation()
        {
            string str = @"
            Here We Are Adding New Column To Extend Database Contact And Column Name Should not Be Found
            in Database Column Names and also should not be any of built in Columns for example as Id or CompanyId
            And If There Any Duplicate Name with Column Name We will get Fail operation
            And Any New Contact is Inserted will Inherit these inserted columns By default
            And You Edit on Them
            And You Should Insert As This Structure Like This Example =>

            {'extendedColumns': {'ExtendedColumnName1YouAdded':'Value','ExtendedColumnName2YouAdded':'Value'}}
            ";
            return str;
        }


        [Route("ContactPostActionExplanationAfterAddColumn")]
        [HttpGet]
        public string ContactPostActionExplanationAfterAddColumn()
        {
            string str = @"
           Here We Are Adding New Contact  After Extending Schema    
           And You Should Insert As This Structure Like This Example =>
        
                [{

                'Name': 'Hayan',
                'Companys': [
                  {

                    'Name': 'Toshiba_NEW_CALM',
                    'NumberOfEmployees': 1000
                  },
                  {

                     'Name': 'AsusNEW',
                    'NumberOfEmployees': 1500
                  }
                ],
                'ExtendedColumnName1YouAdded': 'AssignNewValue',
                'ExtendedColumnName2YouAdded': 'AssignNewValue'


              }]


        
        ";
            return str;
        }
        [Route("ContactPutActionExplanationAfterAddColumn")]
        [HttpGet]
        public string ContactPutActionExplanationAfterAddColumn()
        {
            string str = @"
           Here We Are Editig  Contact  After Extending Schema    
           And You Should Insert As This Structure Like This Example =>
        
             {
    
                'Name': 'Hayan55555',
                'Companys': [
                  {

                     'Name': 'Toshiba_NEW_CALM',
                    'NumberOfEmployees': 1500
                  },
                  {

                     'Name': 'AsusNEW',
                    'NumberOfEmployees': 100
                  }
                ],
                'ExtendedColumnName1YouAdded': 'EditedValue',
                'ExtendedColumnName2YouAdded': 'EditedValue'
              }


        
        ";
            return str;

        }



        [Route("ContactFilterColumnsActionExplanation")]
        [HttpGet]
        public string ContactFilterColumnsActionExplanation()
        {
            string str = @"
            Here We Are Filtering On  Contact Details     
            And You Should Insert As This Structure Like This Example =>
            

                { 'Companys':{'Name':'Toshiba'},'Name': 'Sheikho11','ExtendedColumnNameYouAdded': 'Value'}
            ";
            return str;
        }
    }
}
