using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActualTeast.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }
        public string PlogId { get; set; }
        public Plog Plog { get; set; }
        public string CommenterId { get; set; }
        public User Commenter { get; set; }
    }
}
