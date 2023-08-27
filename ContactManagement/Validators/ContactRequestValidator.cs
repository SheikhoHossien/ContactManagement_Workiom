using ContactManagement.Exceptions;
using ContactManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Validators
{
    public class ContactRequestValidator
    {
        public static void NotFoundContactElement(string message)
        {
            throw new ContactArgumentException($"{message}");
        }
        public static void DuplicatedName(string message)
        {
            throw new ContactArgumentException($"{message} is Duplicated Value");
        }
        public static void Validate(Contact contact)
        {
            int num;
            int num2;
            object num3;
            if (contact == null)
            {
                throw new ContactArgumentException($"{nameof(contact)} is null");
            }
            else if (string.IsNullOrWhiteSpace(contact.Name))
            {
                throw new ContactArgumentException($"{nameof(contact.Name)} is Null/Empty/WhiteSpace");
            }
            /*else if (int.TryParse(company.CompanyId.ToString(), out num))
            {
                throw new CompanyArgumentException($"{nameof(company.CompanyId)} is Invalid");
            }*/
            /*else if (!int.TryParse(contact.NumberOfEmployees.ToString(), out num2))
            {
                throw new ContactArgumentException($"{nameof(contact.NumberOfEmployees)} is Invalid");
            }*/
            if (contact.ExtendedColumns != null)
            {
                if (contact.ExtendedColumns.Count > 0)
                {
                    foreach (var element in contact.ExtendedColumns)
                    {
                        if (string.IsNullOrWhiteSpace(element.Key))
                        {
                            throw new ContactArgumentException($"{nameof(contact.ExtendedColumns)} with Key {nameof(element.Key)} is Null/Empty/WhiteSpace");
                        }
                        if (!contact.ExtendedColumns.TryGetValue(element.Key, out num3))
                        {
                            throw new ContactArgumentException($"There is No Value With This Key {element.Key} ");
                        }
                    }

                }
            }

        }
    }
}
