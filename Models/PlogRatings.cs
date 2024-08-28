namespace ActualTeast.Models
{
    public class PlogRatings
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string PlogId { get; set; }
        public Plog Plog { get; set; }
        public int Rating { get; set; }

    }
}
