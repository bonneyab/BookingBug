using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetEvents : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IEventRepository _eventRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getevents"; } }
        public string Directions { get { return "specify 'getevents -start dd/mm/yyyy -end dd/mm/yyyy' to get events in the specified timerange from booking bug"; } }

        public GetEvents(IBookingBugApi bookingBugApi, IEventRepository eventRepository, IParameterHandler parameterHandler, ILogger logger)
        {   
            _bookingBugApi = bookingBugApi;
            _eventRepository = eventRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            const int expectedTotalArgs = 6;
            var dateArgs = _parameterHandler.GetDateParameters(args, expectedTotalArgs);
            var bbEvents = _bookingBugApi.GetEvents(dateArgs["-start"], dateArgs["-end"]);
            _logger.Info(string.Format("{0} events retrieved successfully.", bbEvents.Count));
            _eventRepository.SaveEvents(bbEvents);
            _logger.Info("Events saved successfully");
        }
    }
}
