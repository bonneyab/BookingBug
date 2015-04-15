using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Contract;
using Core;
using IntegrationServices;
using IntegrationServices.Extensions;
using Newtonsoft.Json.Linq;

namespace BookingBugBookingIntegration
{
    //TODO: the file generation really doesn't belong here, it's just convenient for debugging, consider doing it as some sort of subscription?
    //TODO: Too long, split up? I like that all these silly json related strings are contrained to a couple classes.
    public class BookingBugApi : IBookingBugApi
    {
        private readonly IBookingBugClient _bookingBugClient;
        private readonly IFileGenerationService _fileGenerationService;
        private readonly string _path = ConfigurationManager.AppSettings["JsonFileLocation"];
        private readonly string _perPage = ConfigurationManager.AppSettings["ItemsPerPage"];
        private readonly string _companyId = ConfigurationManager.AppSettings["CompanyId"];

        public BookingBugApi(IBookingBugClient bookingBugClient, IFileGenerationService fileGenerationService)
        {
            _bookingBugClient = bookingBugClient;
            _fileGenerationService = fileGenerationService;
        }

        public Booking GetBooking(int bookingId)
        {
            var jsonResult = _bookingBugClient.GetJson(string.Format("/api/v1/admin/{0}/bookings/{1}", _companyId, bookingId));
            _fileGenerationService.CreateFileFromText(jsonResult.ToString(), _path);

            return CreateBookingFromJson(jsonResult); 
        }

        private Booking CreateBookingFromJson(JToken json)
        {
            var booking = CreateObjectFromJson<Booking>(json);

            var eventJson = json["_links"]["event_chain"];
            if (eventJson != null)
            {
                booking.event_title = Convert.ToString(eventJson["title"]);
                var eventLink = Convert.ToString(eventJson["href"]);
                var pos = eventLink.LastIndexOf("/", StringComparison.Ordinal) + 1;
                booking.event_id = eventLink.Substring(pos, eventLink.Length - pos);
            }
            var questionGroupingJson = json["_links"]["questions"];
            if (questionGroupingJson != null)
            {
                //TODO: duplicate of above - refactor
                var questionGroupLink = Convert.ToString(questionGroupingJson["href"]);
                var pos = questionGroupLink.LastIndexOf("=", StringComparison.Ordinal) + 1;
                booking.question_group_id = questionGroupLink.Substring(pos, questionGroupLink.Length - pos);
            }
            if (json["_embedded"]["answers"] == null) return booking;

            foreach (var answer in JArray.Parse(json["_embedded"]["answers"].ToString()))
            {
                booking.Answers.Add(CreateObjectFromJson<Answer>(answer));
                booking.Questions.Add(CreateObjectFromJson<Question>(answer["_embedded"]["question"]));
            }
            return booking;
        }

        public List<Booking> GetBookings(DateTime modified, DateTime start, DateTime end)
        {
            var jsonResult =
                _bookingBugClient.GetPagedJson(string.Format("/api/v1/admin/{0}/bookings?modified_since={1}&start_date={2}&end_date={3}&page=1&per_page={4}", _companyId,
                    modified.ToApiFormat(), start.ToApiFormat(), end.ToApiFormat(), _perPage));
            _fileGenerationService.CreateFileFromText(string.Join("\r\n", jsonResult.Select(j => j.ToString())), _path);

            var bookings = new List<Booking>();
            foreach (var bookingsJson in jsonResult.Select(j => JArray.Parse(j["_embedded"]["bookings"].ToString())))
            {
                bookings.AddRange(bookingsJson.Select(CreateBookingFromJson));
            }
            return bookings;
        }

        public List<Event> GetEvents(DateTime start, DateTime end)
        {
            var jsonResult =
                _bookingBugClient.GetPagedJson(string.Format("/api/v1/admin/{0}/event_chains?start_date={1}&end_date={2}&page=1&per_page={3}", _companyId, 
                    start.ToApiFormat(), end.ToApiFormat(), _perPage));
            _fileGenerationService.CreateFileFromText(string.Join("\r\n", jsonResult.Select(j => j.ToString())), _path);

            var events = new List<Event>();
            foreach (var eventssJson in jsonResult.Select(j => JArray.Parse(j["_embedded"]["event_chains"].ToString())))
            {
                events.AddRange(eventssJson.Select(CreateObjectFromJson<Event>));
            }
            return events;
        }

        public Event GetEvent(int eventId)
        {
            var jsonResult = _bookingBugClient.GetJson(string.Format("/api/v1/admin/{0}/event_chains/{1}", _companyId, eventId));
            _fileGenerationService.CreateFileFromText(jsonResult.ToString(), _path);

            return CreateObjectFromJson<Event>(jsonResult); 
        }

        //yeah yeah, so generic it's hard to use, bit of a one-off though.
        public void PostData(string type, Dictionary<string, string> postData)
        {
            var jsonResult =
                _bookingBugClient.PostData(string.Format("/api/v1/admin/{0}/{1}", _companyId, type),
                    postData);
            _fileGenerationService.CreateFileFromText(jsonResult.ToString(), _path);
        }

        public Client GetClient(int clientId)
        {
            var jsonResult = _bookingBugClient.GetJson(string.Format("/api/v1/admin/{0}/client/{1}", _companyId, clientId));
            _fileGenerationService.CreateFileFromText(jsonResult.ToString(), _path);

            return CreateObjectFromJson<Client>(jsonResult); 
        }

        public List<Client> GetClients()
        {
            var jsonResult =
                _bookingBugClient.GetPagedJson(string.Format("/api/v1/admin/{0}/client?page=1&per_page={1}", _companyId, _perPage));
            _fileGenerationService.CreateFileFromText(string.Join("\r\n", jsonResult.Select(j => j.ToString())), _path);

            var clients = new List<Client>();
            foreach (var clientsJson in jsonResult.Select(j => JArray.Parse(j["_embedded"]["clients"].ToString())))
            {
                clients.AddRange(clientsJson.Select(CreateObjectFromJson<Client>));
            }
            return clients;
        }

        public Person GetPerson(int personId)
        {
            var jsonResult = _bookingBugClient.GetJson(string.Format("/api/v1/admin/{0}/people/{1}", _companyId, personId));
            _fileGenerationService.CreateFileFromText(jsonResult.ToString(), _path);

            return CreateObjectFromJson<Person>(jsonResult); 
        }

        public List<Person> GetPeople()
        {
            var jsonResult =
                _bookingBugClient.GetPagedJson(string.Format("/api/v1/admin/{0}/people?page=1&per_page={1}", _companyId, _perPage));
            _fileGenerationService.CreateFileFromText(string.Join("\r\n", jsonResult.Select(j => j.ToString())), _path);

            var people = new List<Person>();
            foreach (var clientsJson in jsonResult.Select(j => JArray.Parse(j["_embedded"]["people"].ToString())))
            {
                people.AddRange(clientsJson.Select(CreateObjectFromJson<Person>));
            }
            return people;
        }

        //one could complain about the reflection in the loop, there just really isn't that much data flowing through this thing though.
        //It would be easy enough to pull the properties out
        public T CreateObjectFromJson<T>(JToken json) where T : new()
        {
            var properties = typeof(T).GetProperties();
            var item = new T();
            foreach (var propertyInfo in properties)
            {
                //TODO: Try catch log?
                propertyInfo.SetValue(item, Convert.ToString(json[propertyInfo.Name]));
            }
            return item;
        }
    }

    public interface IBookingBugApi : IDependency
    {
        Booking GetBooking(int bookingId);
        List<Booking> GetBookings(DateTime modified, DateTime start, DateTime end);
        List<Event> GetEvents(DateTime start, DateTime end);
        Event GetEvent(int eventId);
        List<Person> GetPeople();
        Person GetPerson(int personId);
        Client GetClient(int clientId);
        List<Client> GetClients();
        void PostData(string type, Dictionary<string, string> postData);
    }
}