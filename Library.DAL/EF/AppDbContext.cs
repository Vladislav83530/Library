using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.EF
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
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

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "To Kill a Mockingbird", Content = "Lorem ipsum...", Author = "Harper Lee", Genre = "novel", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 2, Title = "1984", Content = "Lorem ipsum...", Author = "George Orwell", Genre = "classic", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 3, Title = "Pride and Prejudice", Content = "Lorem ipsum...", Author = "Jane Austen", Genre = "romance", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 4, Title = "The Great Gatsby", Content = "Lorem ipsum...", Author = "F. Scott Fitzgerald", Genre = "novel ", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 5, Title = "One Hundred Years of Solitude", Content = "Lorem ipsum...", Author = "Gabriel Garcia Marquez", Genre = "novel", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 6, Title = "The Catcher in the Rye", Content = "Lorem ipsum...", Author = "J.D. Salinger", Genre = "novel", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 7, Title = "Dracula", Content = "Lorem ipsum...", Author = "Bram Stoker", Genre = "horror", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 8, Title = "The Silence of the Lambs", Content = "Lorem ipsum...", Author = "Thomas Harris", Genre = "horror", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 9, Title = "Rosemary's Baby", Content = "Lorem ipsum...", Author = "Ira Levin", Genre = "horror", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 10, Title = "The Haunting of Hill House", Content = "Lorem ipsum...", Author = "Shirley Jackson", Genre = "horror", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." },
                new Book { Id = 11, Title = "The Turn of the Screw", Content = "Lorem ipsum...", Author = "Henry James", Genre = "horror", Cover = "data:image/png;base64,iVBORw0KGg+rR9wo1b1l/F63P..." }
                );

            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Ayrton Whitehead" },
                new Review { Id = 2, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Madiha Mccann" },
                new Review { Id = 3, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 4, Message = "Lorem ipsum...", BookId = 2, Reviewer = "Ayrton Whitehead" },
                new Review { Id = 5, Message = "Lorem ipsum...", BookId = 2, Reviewer = "Rihanna Woodward" },
                new Review { Id = 6, Message = "Lorem ipsum...", BookId = 3, Reviewer = "Ronald Anderson" },
                new Review { Id = 7, Message = "Lorem ipsum...", BookId = 4, Reviewer = "Kabir Fisher" },
                new Review { Id = 8, Message = "Lorem ipsum...", BookId = 5, Reviewer = "Ronald Anderson" },
                new Review { Id = 9, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 10, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Kabir Fisher" },
                new Review { Id = 11, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 12, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Marwa Ponce" },
                new Review { Id = 13, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 14, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Ronald Anderson" },
                new Review { Id = 15, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 16, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" },
                new Review { Id = 17, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Jeremy Campos" },
                new Review { Id = 18, Message = "Lorem ipsum...", BookId = 1, Reviewer = "Rihanna Woodward" }
                );

            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, BookId = 1, Score = 5 },
                new Rating { Id = 2, BookId = 1, Score = 4 },
                new Rating { Id = 3, BookId = 1, Score = 4 },
                new Rating { Id = 4, BookId = 2, Score = 3 },
                new Rating { Id = 5, BookId = 2, Score = 5 },
                new Rating { Id = 6, BookId = 3, Score = 2 },
                new Rating { Id = 7, BookId = 4, Score = 4 },
                new Rating { Id = 8, BookId = 5, Score = 3 }
                );
        }
    }
}
