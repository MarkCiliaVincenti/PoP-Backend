using System;

namespace PopLibrary.SqlModels
{
    public class Event
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Goal { get; set; }

        public string Type { get; set; }

        public decimal BaseAmount { get; set; }

        public string Venue { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

    }
}
