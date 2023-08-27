using ContactManagement.Exceptions;
using ContactManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Validators
{
    public static class CompanyRequestValidator
    {
        public static void NotFoundCompanyElement(string message)
        {
            throw new CompanyArgumentException($"{message}");
        }
        public static void DuplicatedName(string message)
        {
            throw new CompanyArgumentException($"{message} is Duplicated Value");
        }
        public static void Validate(Company  company )
        {
            int num;
            int num2;
            object num3;
            if (company==null)
            {
                throw new CompanyArgumentException($"{nameof(company)} is null");
            }
            else if(string.IsNullOrWhiteSpace(company.Name))
            {
                throw new CompanyArgumentException($"{nameof(company.Name)} is Null/Empty/WhiteSpace");
            }
            /*else if (int.TryParse(company.CompanyId.ToString(), out num))
            {
                throw new CompanyArgumentException($"{nameof(company.CompanyId)} is Invalid");
            }*/
            else if (!int.TryParse(company.NumberOfEmployees.ToString(), out num2))
            {
                throw new CompanyArgumentException($"{nameof(company.NumberOfEmployees)} is Invalid");
            }
            if(company.ExtendedColumns!=null)
            {
                if (company.ExtendedColumns.Count > 0)
                {
                    foreach(var element in company.ExtendedColumns)
                    {
                        if (string.IsNullOrWhiteSpace(element.Key))
                        {
                            throw new CompanyArgumentException($"{nameof(company.ExtendedColumns)} with Key {nameof(element.Key)} is Null/Empty/WhiteSpace");
                        }
                        if (!company.ExtendedColumns.TryGetValue(element.Key,out num3))
                        {
                            throw new CompanyArgumentException($"There is No Value With This Key {element.Key} ");
                        }
                    }

                }
            }
            
        }

    }
}
