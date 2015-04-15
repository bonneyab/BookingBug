using System.Collections.Generic;
using System.Linq;
using BookingBugBookingIntegration.BookingBugOperations;
using Core;
using IntegrationServices.Interfaces;

namespace BookingBugBookingIntegration
{
    public class BookingBugBookingIntegration : IIntegration
    {
        private readonly List<IBookingBugOperation> _bookingBugOperations;
        private readonly ILogger _logger;

        public string IntegrationName
        {
            get { return "bookingbug"; }
        }

        public string Directions
        {
            get { return "Specify \"bookingbug\" and any additional arguments: \r\n" + string.Join("\r\n", _bookingBugOperations.Select(b => b.Directions)); }
        }

        public BookingBugBookingIntegration(IEnumerable<IBookingBugOperation> bookingBugOperations
            ,ILogger logger)
        {
            _bookingBugOperations = bookingBugOperations.ToList();
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            _logger.Info("Starting Booking Bug Booking execution");
            _bookingBugOperations.First(b => b.Name == args[1]).Execute(args);
            _logger.Info("Finished Booking Bug Booking execution");
        }
    }
}