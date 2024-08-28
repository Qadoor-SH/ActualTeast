using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActualTeast.Models
{
    public class Plog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PlogRatings>? Ratings { get; set; }
        public int GetTotalRating()
        {
            int totalRating = 0;
            foreach (var rateing in Ratings)
            {
                totalRating += rateing.Rating;
            }
            if(Ratings.Count>0)
            {
            return totalRating/ Ratings.Count;

            }
            return 0;
        }
    }
}
