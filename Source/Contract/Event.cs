namespace Contract
{
    //Names need to match Booking bug names
    public class Event
    {
        public string min_advance_time { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public string group { get; set; }
        public string time { get; set; }
        public string long_description { get; set; }
        public string capacity_view { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string spaces { get; set; }
        public string person_name { get; set; }
        public string price { get; set; }
        public string max_num_bookings { get; set; }
        public string ticket_type { get; set; }
        public string email_per_ticket { get; set; }
        public string questions_per_ticket { get; set; }
        public string course { get; set; }
    }
}
