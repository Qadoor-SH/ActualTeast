using Microsoft.AspNetCore.Identity;

namespace ActualTeast.Models
{
    public class User: IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Plog>? Plogs { get; set; }
        public ICollection<PlogRatings>? Ratings { get; set; }
    }
}
