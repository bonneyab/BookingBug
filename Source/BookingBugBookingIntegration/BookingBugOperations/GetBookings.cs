using Core;
using DataAccess;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetBookings : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IBookingRepository _bookingRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getbookings"; } }
        public string Directions { get { return "specify 'getbookings -modified dd/mm/yyyy -start dd/mm/yyyy -end dd/mm/yyyy' this will get all bookings in the specified timerange that have been modified since the specified time."; } }

        public GetBookings(IBookingBugApi bookingBugApi, IBookingRepository bookingRepository, IParameterHandler parameterHandler, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _bookingRepository = bookingRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            const int expectedArgs = 8;
            var dateArgs = _parameterHandler.GetDateParameters(args, expectedArgs);
            var bookings = _bookingBugApi.GetBookings(dateArgs["-modified"], dateArgs["-start"], dateArgs["-end"]);
            _logger.Info(string.Format("{0} bookings retrieved", bookings.Count));
            _bookingRepository.SaveBookings(bookings);
            _logger.Info("Bookings saved successfully.");
        }
    }
}
