using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActualTeast.Models
{
    public class ApplicationDBContext: IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Plog> Plogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PlogRatings> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Plog>()
            .HasOne(b => b.Owner)
            .WithMany(b => b.Plogs)
            .HasForeignKey(b => b.OwnerId)
            .IsRequired();
            builder.Entity<Comment>()
            .HasOne(b => b.Plog)
            .WithMany(b => b.Comments)
            .HasForeignKey(b => b.PlogId)
            .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>()
            .HasOne(b => b.Commenter)
            .WithMany(b => b.Comments)
            .HasForeignKey(b => b.CommenterId)
            .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>()
            .HasOne(b => b.Commenter)
            .WithMany(b => b.Comments)
            .HasForeignKey(b => b.CommenterId)
            .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PlogRatings>()
                .HasKey(b => new { b.UserId,b.PlogId });

            builder.Entity<PlogRatings>()
            .HasOne(b => b.User)
            .WithMany(b => b.Ratings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PlogRatings>()
            .HasOne(b => b.Plog)
            .WithMany(b => b.Ratings)
            .HasForeignKey(b => b.PlogId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
