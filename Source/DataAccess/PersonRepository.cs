using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace DataAccess
{
    class PersonRepository : IPersonRepository
    {
        readonly ReportingEntities _context = new ReportingEntities();

        public void SavePeople(List<Contract.Person> contractPeople)
        {
            contractPeople.ForEach(AddPersonToContext);
            _context.SaveChanges();
        }

        public void SavePerson(Contract.Person contractPerson)
        {
            AddPersonToContext(contractPerson);
            _context.SaveChanges();
        }

        private void AddPersonToContext(Contract.Person contractPerson)
        {
            var bbPersonId = Convert.ToInt32(contractPerson.id);
            var existingPerson = _context.People.FirstOrDefault(b => b.Id == bbPersonId);
            Action<Exception> errorHandler = ex => { throw new Exception(string.Format("Unable to create bbPerson from API bbPerson for bbPersonID: {0}", contractPerson.id), ex); };
            try
            {
                var bbPerson = GetPersonFromContract(contractPerson, existingPerson);
                if (existingPerson == null)
                    _context.People.Add(bbPerson);
            }
            catch (FormatException ex)
            {
                errorHandler.Invoke(ex);
            }
            catch (InvalidOperationException ex)
            {
                errorHandler.Invoke(ex);
            }
        }

        private Person GetPersonFromContract(Contract.Person bbPersonContract, Person efPerson = null)
        {
            Func<string, int> convertIntWithDefault = c => Convert.ToInt32(string.IsNullOrEmpty(c) ? "0" : c);
            var bbPerson = efPerson ?? new Person();
            bbPerson.CreatedBy = Environment.UserName;
            bbPerson.CreatedDate = DateTime.Now;
            bbPerson.ModifiedBy = Environment.UserName;
            bbPerson.ModifiedDate = DateTime.Now;
            bbPerson.Id = Convert.ToInt32(bbPersonContract.id);
            bbPerson.name = bbPersonContract.name;
            bbPerson.company_id = convertIntWithDefault(bbPersonContract.company_id);
            bbPerson.deleted = Convert.ToBoolean(bbPersonContract.deleted);
            bbPerson.disabled = Convert.ToBoolean(bbPersonContract.disabled);
            bbPerson.email = bbPersonContract.email;
            bbPerson.mobile = bbPersonContract.mobile;
            bbPerson.type = bbPersonContract.type;
            return bbPerson;
        }
    }

    public interface IPersonRepository : IDependency
    {
        void SavePerson(Contract.Person contractPerson);
        void SavePeople(List<Contract.Person> contractPeople);
    }
}
