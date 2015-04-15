using Core;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    public interface IBookingBugOperation : IDependency
    {
        string Name { get; }
        string Directions { get; }
        void Execute(string[] args);
    }
}
