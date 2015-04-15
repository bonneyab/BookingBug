using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace DataAccess
{
    class EventRepository : IEventRepository
    {
        readonly ReportingEntities _context = new ReportingEntities();

        public void SaveEvents(List<Contract.Event> contractEvents)
        {
            contractEvents.ForEach(AddEventToContext);
            _context.SaveChanges();
        }

        public void SaveEvent(Contract.Event contractEvent)
        {
            AddEventToContext(contractEvent);
            _context.SaveChanges();
        }

        private void AddEventToContext(Contract.Event contractEvent)
        {
            var bbEventId = Convert.ToInt32(contractEvent.id);
            var existingEvent = _context.Events.FirstOrDefault(b => b.Id == bbEventId);
            Action<Exception> errorHandler = ex => { throw new Exception(string.Format("Unable to create bbEvent from API bbEvent for bbEventID: {0}", contractEvent.id), ex); };
            try
            {
                var bbEvent = GetEventFromContract(contractEvent, existingEvent);
                if (existingEvent == null)
                    _context.Events.Add(bbEvent);
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

        private Event GetEventFromContract(Contract.Event bbEventContract, Event efEvent = null)
        {
            Func<string, int> convertIntWithDefault = c => Convert.ToInt32(string.IsNullOrEmpty(c) ? "0" : c);
            var bbEvent = efEvent ?? new Event();
            bbEvent.CreatedBy = Environment.UserName;
            bbEvent.CreatedDate = DateTime.Now;
            bbEvent.ModifiedBy = Environment.UserName;
            bbEvent.ModifiedDate = DateTime.Now;
            bbEvent.Id = Convert.ToInt32(bbEventContract.id);
            bbEvent.name = bbEventContract.name;
            bbEvent.description = bbEventContract.description;
            bbEvent.duration = convertIntWithDefault(bbEventContract.duration);
            bbEvent.group = bbEventContract.group;
            bbEvent.time = Convert.ToDateTime(bbEventContract.time);
            bbEvent.long_description = bbEventContract.long_description;
            bbEvent.capacity_view = convertIntWithDefault(bbEventContract.capacity_view);
            bbEvent.start_date =  Convert.ToDateTime(bbEventContract.start_date);
            bbEvent.end_date =  Convert.ToDateTime(bbEventContract.end_date);
            bbEvent.spaces = convertIntWithDefault(bbEventContract.spaces);
            bbEvent.person_name = bbEventContract.person_name;
            bbEvent.price = Convert.ToDecimal(bbEventContract.price);
            bbEvent.max_num_bookings = convertIntWithDefault(bbEventContract.max_num_bookings);
            bbEvent.min_advance_time = Convert.ToDateTime(bbEventContract.min_advance_time);
            bbEvent.ticket_type = bbEventContract.ticket_type;
            bbEvent.email_per_ticket = Convert.ToBoolean(bbEventContract.email_per_ticket);
            bbEvent.questions_per_ticket = Convert.ToBoolean(bbEventContract.questions_per_ticket);
            bbEvent.course = Convert.ToBoolean(bbEventContract.course);
            return bbEvent;
        }
    }

    public interface IEventRepository : IDependency
    {
        void SaveEvent(Contract.Event contractEvent);
        void SaveEvents(List<Contract.Event> contractEvents);
    }
}
