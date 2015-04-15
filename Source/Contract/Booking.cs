using System.Collections.Generic;

namespace Contract
{
    //Names need to match Booking bug names
    public class Booking
    {
        public string id { get; set; }
        public string full_describe { get; set; }
        public string person_name { get; set; }
        public string service_name { get; set; }
        public string service_id { get; set; }
        public string member_id { get; set; }
        public string client_name { get; set; }
        public string client_email { get; set; }
        public string client_phone { get; set; }
        public string client_mobile { get; set; }
        public string datetime { get; set; }
        public string duration { get; set; }
        public string on_waitlist { get; set; }
        public string company_id { get; set; }
        public string attended { get; set; }
        public string booking_updated { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public string client_id { get; set; }
        public string person_id { get; set; }
        public string price { get; set; }
        public string paid { get; set; }
        public string quantity { get; set; }
        public string is_cancelled { get; set; }
        public string channel { get; set; }
        public string status { get; set; }
        public string event_id { get; set; }
        public string event_title { get; set; }
        public string question_group_id { get; set; }
        public List<Answer> Answers = new List<Answer>();
        public List<Question> Questions = new List<Question>(); 
    }

    public class Answer
    {
        public string id { get; set; }
        public string question_id { get;set; }
        public string value { get; set; }
    }

    public class Question
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
