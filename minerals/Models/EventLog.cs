using System;

namespace Models
{
    public class EventLog
    {
        public long Id { get; set; }
        public string EventMessage { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string StatusCode { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
