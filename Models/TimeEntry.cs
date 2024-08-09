namespace RareServer.Models
{
    public class TimeEntry
    {
        public Guid Id { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string? EntryNotes { get; set; }
        public DateTime? DeletedOn { get; set; }

        public decimal TotalTimeWorked
        {
            get
            {
                var duration = (decimal)(EndTimeUtc - StarTimeUtc).TotalHours;
                return Math.Round(duration >= 0 ? duration : 0);
            }
        }
    }
}