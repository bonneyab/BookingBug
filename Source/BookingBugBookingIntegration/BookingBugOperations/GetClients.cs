using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetClients : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger _logger;

        public string Name { get { return "getclients"; } }
        public string Directions { get { return "specify 'getclients' to get all clients from booking bug"; } }

        public GetClients(IBookingBugApi bookingBugApi, IClientRepository clientRepository, ILogger logger)
        {   
            _bookingBugApi = bookingBugApi;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var clients = _bookingBugApi.GetClients();
            _logger.Info(string.Format("{0} clients retrieved successfully.", clients.Count));
            _clientRepository.SaveClients(clients);
            _logger.Info("Clients saved successfully");
        }
    }
}
