using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace DataAccess
{
    //TODO: consider a more generic repository pattern?
    //TODO: consider moving the interfaces into the contract project?
    class BookingRepository : IBookingRepository
    {
        private readonly ILogger _logger;
        readonly ReportingEntities _context = new ReportingEntities();
        //TODO: this thing is in a few places and is a bit goofy?
        Func<string, int> convertIntWithDefault = c => Convert.ToInt32(string.IsNullOrEmpty(c) ? "0" : c);

        public BookingRepository(ILogger logger)
        {
            _logger = logger;
        }

        public void SaveBookings(List<Contract.Booking> contractBookings)
        {
            contractBookings.ForEach(AddBookingToContext);
            _context.SaveChanges();
            contractBookings.ForEach(AddBookingQuestionsToContext);
            _context.SaveChanges();
        }

        public void SaveBooking(Contract.Booking contractBooking)
        {
            AddBookingToContext(contractBooking);
            _context.SaveChanges();
            AddBookingQuestionsToContext(contractBooking);
            _context.SaveChanges();
        }

        private void AddBookingQuestionsToContext(Contract.Booking contractBooking)
        {
            foreach (var answer in contractBooking.Answers)
            {
                var answerId = Convert.ToInt32(answer.id);
                var questionId = convertIntWithDefault(answer.question_id);
                var existingAnswer = _context.QuestionResults.FirstOrDefault(b => b.answer_id == answerId);
                var questionresult = existingAnswer ?? new QuestionResult();
                questionresult.answer_id = answerId;
                questionresult.answer_text = answer.value;
                questionresult.question_id = questionId;
                var question = contractBooking.Questions.First(q => q.id == answer.question_id);
                questionresult.question_text = question.name;
                questionresult.booking_id = convertIntWithDefault(contractBooking.id);
                questionresult.question_group_id = convertIntWithDefault(contractBooking.question_group_id);
                questionresult.CreatedBy = Environment.UserName;
                questionresult.CreatedDate = DateTime.Now;
                questionresult.ModifiedBy = Environment.UserName;
                questionresult.ModifiedDate = DateTime.Now;
                if(existingAnswer == null)
                    _context.QuestionResults.Add(questionresult);
            }
        }

        private void AddBookingToContext(Contract.Booking contractBooking)
        {
            var bookingId = Convert.ToInt32(contractBooking.id);
            var existingBooking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            Action<Exception> errorHandler = ex => { _logger.Error(string.Format("Unable to create booking from API booking for bookingID: {0} Skipping", contractBooking.id), ex); };
            try
            {
                var booking = GetBookingFromContract(contractBooking, existingBooking);
                if (existingBooking == null)
                    _context.Bookings.Add(booking);
            }
            catch (FormatException ex)
            {
                errorHandler.Invoke(ex);
            }
            catch (InvalidOperationException ex)
            {
                errorHandler.Invoke(ex);
            }
        }

        //TODO: consider using automapper for this sort of stuff?
        private Booking GetBookingFromContract(Contract.Booking bookingContract, Booking efBooking = null)
        {
            //I'm not a fan of taking that entity framework booking into here?
            var booking = efBooking ?? new Booking();
            booking.attended = Convert.ToBoolean(bookingContract.attended);
            booking.booking_updated = Convert.ToDateTime(bookingContract.booking_updated);
            booking.channel = bookingContract.channel;
            booking.client_email = bookingContract.client_email;
            booking.client_id = convertIntWithDefault(bookingContract.client_id);
            booking.client_mobile =bookingContract.client_mobile;
            booking.client_name =bookingContract.client_name;
            booking.client_phone =bookingContract.client_phone;
            booking.company_id = convertIntWithDefault(bookingContract.company_id);
            booking.created_at = Convert.ToDateTime(bookingContract.created_at);
            booking.CreatedBy = Environment.UserName;
            booking.CreatedDate = DateTime.Now;
            booking.ModifiedBy = Environment.UserName;
            booking.ModifiedDate = DateTime.Now;
            booking.event_id = convertIntWithDefault(bookingContract.event_id);
            booking.event_title = bookingContract.event_title;
            booking.Id = Convert.ToInt32(bookingContract.id);
            booking.member_id = convertIntWithDefault(bookingContract.member_id);
            booking.paid = Convert.ToDecimal(bookingContract.paid);
            booking.price = Convert.ToDecimal(bookingContract.price);
            booking.person_id = convertIntWithDefault(bookingContract.person_id);
            booking.person_name = bookingContract.person_name;
            booking.is_cancelled = Convert.ToBoolean(bookingContract.is_cancelled);
            booking.updated_at = Convert.ToDateTime(bookingContract.updated_at);
            booking.datetime = Convert.ToDateTime(bookingContract.datetime);
            booking.duration = convertIntWithDefault(bookingContract.duration);
            booking.full_describe = bookingContract.full_describe;
            booking.service_id = convertIntWithDefault(bookingContract.service_id);
            booking.status = convertIntWithDefault(bookingContract.status);
            booking.service_name = bookingContract.service_name;
            booking.quantity = Convert.ToInt32(bookingContract.quantity);
            return booking;
        }
    }

    public interface IBookingRepository : IDependency
    {
        void SaveBooking(Contract.Booking contractBooking);
        void SaveBookings(List<Contract.Booking> contractBookings);
    }
}
