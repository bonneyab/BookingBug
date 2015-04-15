using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetEvent : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IEventRepository _eventRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getevent"; } }
        public string Directions { get { return "specify 'getevent' along with the event id ex: 'getevent 1"; } }

        public GetEvent(IBookingBugApi bookingBugApi, IEventRepository eventRepository, IParameterHandler parameterHandler, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _eventRepository = eventRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var eventId = _parameterHandler.GetIdFromParams(args);
            var bbEvent = _bookingBugApi.GetEvent(eventId);
            _logger.Info("Event retrieved successfully.");
            _eventRepository.SaveEvent(bbEvent);
            _logger.Info("Event saved successfully.");

        }
    }
}
