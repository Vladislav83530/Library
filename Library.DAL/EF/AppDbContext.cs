using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.EF
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }  
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "LibraryDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Review>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Rating>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Reviews)
                .WithOne(x => x.Book)
                .HasForeignKey(x => x.BookId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Ratings)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId);
        }
    }
}
