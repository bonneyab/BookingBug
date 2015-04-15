using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace DataAccess
{
    class ClientRepository : IClientRepository
    {
        readonly ReportingEntities _context = new ReportingEntities();

        public void SaveClients(List<Contract.Client> contractClients)
        {
            contractClients.ForEach(AddClientToContext);
            _context.SaveChanges();
        }

        public void SaveClient(Contract.Client contractClient)
        {
            AddClientToContext(contractClient);
            _context.SaveChanges();
        }

        private void AddClientToContext(Contract.Client contractClient)
        {
            var bbClientId = Convert.ToInt32(contractClient.id);
            var existingClient = _context.Clients.FirstOrDefault(b => b.Id == bbClientId);
            Action<Exception> errorHandler = ex => { throw new Exception(string.Format("Unable to create bbClient from API bbClient for bbClientID: {0}", contractClient.id), ex); };
            try
            {
                var bbClient = GetClientFromContract(contractClient, existingClient);
                if (existingClient == null)
                    _context.Clients.Add(bbClient);
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

        private Client GetClientFromContract(Contract.Client bbClientContract, Client efClient = null)
        {
            var bbClient = efClient ?? new Client();
            bbClient.CreatedBy = Environment.UserName;
            bbClient.CreatedDate = DateTime.Now;
            bbClient.ModifiedBy = Environment.UserName;
            bbClient.ModifiedDate = DateTime.Now;
            bbClient.Id = Convert.ToInt32(bbClientContract.id);
            bbClient.first_name = bbClientContract.first_name;
            bbClient.last_name = bbClientContract.last_name;
            bbClient.email = bbClientContract.email;
            bbClient.address1 = bbClientContract.address1;
            bbClient.address2 = bbClientContract.address2;
            bbClient.address3 = bbClientContract.address3;
            bbClient.address4 = bbClientContract.address4;
            bbClient.address5 = bbClientContract.address5;
            bbClient.postcode = bbClientContract.postcode;
            bbClient.country = bbClientContract.country;
            bbClient.mobile = bbClientContract.mobile;
            bbClient.member_type = bbClientContract.member_type;
            bbClient.deleted = Convert.ToBoolean(bbClientContract.deleted);
            bbClient.mobile_prefix = bbClientContract.mobile_prefix;
            bbClient.phone_prefix = bbClientContract.phone_prefix;
            return bbClient;
        }
    }

    public interface IClientRepository : IDependency
    {
        void SaveClient(Contract.Client contractClient);
        void SaveClients(List<Contract.Client> contractPeople);
    }
}
