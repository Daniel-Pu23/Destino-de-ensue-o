using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DestinoEnsueno.API.Models;

namespace DestinoEnsueno.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Índice único: un usuario solo puede tener una reseña por negocio
            builder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.BusinessId })
                .IsUnique();

            // Restricción: Rating entre 1 y 5
            builder.Entity<Review>()
                .ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "Rating >= 1 AND Rating <= 5"));

            // Relaciones

            builder.Entity<Business>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Businesses)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Business)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Image>()
                .HasOne(i => i.Business)
                .WithMany(b => b.Images)
                .HasForeignKey(i => i.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasOne(e => e.Business)
                .WithMany(b => b.Events)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Event>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            builder.Entity<Discount>()
                .HasOne(d => d.Business)
                .WithMany(b => b.Discounts)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
