using Core;
using IntegrationDataServices;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetPeople: IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger _logger;

        public string Name { get { return "getpeople"; } }
        public string Directions { get { return "specify 'getpeople' to get all people (VC staff) from booking bug"; } }

        public GetPeople(IBookingBugApi bookingBugApi, IPersonRepository personRepository, ILogger logger)
        {   
            _bookingBugApi = bookingBugApi;
            _personRepository = personRepository;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var bbEvents = _bookingBugApi.GetPeople();
            _logger.Info(string.Format("{0} people retrieved successfully.", bbEvents.Count));
            _personRepository.SavePeople(bbEvents);
            _logger.Info("People saved successfully");
        }
    }
}
