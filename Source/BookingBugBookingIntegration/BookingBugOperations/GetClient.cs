using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetClient: IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IClientRepository _clientRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getclient"; } }
        public string Directions { get { return "specify 'getclient' along with the clientid id ex: 'getclient 1"; } }

        public GetClient(IBookingBugApi bookingBugApi, IClientRepository clientRepository, IParameterHandler parameterHandler, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _clientRepository = clientRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var clientId = _parameterHandler.GetIdFromParams(args);
            var client = _bookingBugApi.GetClient(clientId);
            _logger.Info("Client retrieved successfully.");
            _clientRepository.SaveClient(client);
            _logger.Info("Client saved successfully.");

        }
    }
}
