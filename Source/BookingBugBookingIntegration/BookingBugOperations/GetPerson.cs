using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetPerson : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IPersonRepository _personRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getperson"; } }
        public string Directions { get { return "specify 'getperson' along with the personid id ex: 'getperson 1"; } }

        public GetPerson(IBookingBugApi bookingBugApi, IPersonRepository personRepository, IParameterHandler parameterHandler, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _personRepository = personRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var personId = _parameterHandler.GetIdFromParams(args);
            var person = _bookingBugApi.GetPerson(personId);
            _logger.Info("Person retrieved successfully.");
            _personRepository.SavePerson(person);
            _logger.Info("Person saved successfully.");

        }
    }
}
