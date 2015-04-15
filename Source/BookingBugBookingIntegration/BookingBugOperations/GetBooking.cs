using Core;
using IntegrationDataServices;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    class GetBooking : IBookingBugOperation
    {
        private readonly IBookingBugApi _bookingBugApi;
        private readonly IBookingRepository _bookingRepository;
        private readonly IParameterHandler _parameterHandler;
        private readonly ILogger _logger;

        public string Name { get { return "getbooking"; } }
        public string Directions { get { return "specify 'getbooking' along with the booking id ex 'getbooking 1'"; } }

        public GetBooking(IBookingBugApi bookingBugApi, IBookingRepository bookingRepository, IParameterHandler parameterHandler, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _bookingRepository = bookingRepository;
            _parameterHandler = parameterHandler;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            var bookingId = _parameterHandler.GetIdFromParams(args);
            var booking = _bookingBugApi.GetBooking(bookingId);
            _logger.Info("Booking retrieved successfully");
            _bookingRepository.SaveBooking(booking);
            _logger.Info("Booking saved successfully");
        }
    }
}
