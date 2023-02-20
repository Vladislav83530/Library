using AutoMapper;
using Library.BLL.DTOs;
using Library.BLL.Services.Interfaces;
using Library.DAL.EF;
using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Library.BLL.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LibraryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all books by order (title or author)
        /// </summary>
        /// <param name="order"></param>
        /// <returns>list of books with rating info or exception</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync(string order)
        {
            if (String.IsNullOrEmpty(order))
                throw new Exception("Order cann't be null or empty");

            var books = await _context.Books
                .Include(x => x.Ratings)
                .Include(x => x.Reviews).ToListAsync();


            if (order.ToLower() == "author")
            {
                books = books.OrderBy(x => x.Author).ToList();
                return _mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
            }
            else if (order.ToLower() == "title")
            {
                books = books.OrderBy(x => x.Title).ToList();
                return _mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
            }
            else
                throw new Exception("Order parameter can be only 'author' or 'title'");
        }

        /// <summary>
        ///  Get top 10 books with high rating and number of reviews greater than 10
        ///  Filter books by specifying genre. 
        ///  Order by rating
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>List of books with rating info</returns>
        public async Task<IEnumerable<BookDTO>> GetRecommendedBooksAsync(string? genre)
        {
            var books = _context.Books
                .Include(b => b.Ratings)
                .Include(b => b.Reviews);

            var recommendBooks = books
                .Where(x => x.Reviews.Count() >= 10)
                .OrderByDescending(x => x.Ratings.Average(r => r.Score))
                .Take(10);

            if (!String.IsNullOrEmpty(genre))
                recommendBooks = recommendBooks.Where(x => x.Genre == genre);

            var result = await recommendBooks.ToListAsync();

            return _mapper.Map<IEnumerable<Book>, List<BookDTO>>(result);
        }

        /// <summary>
        /// Get book details with the list of reviews
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Book details with the list of reviews</returns>
        public async Task<BookDetailsDTO> GetBookDetailsAsync(int Id)
        {
            var book = await _context.Books
                .Include(b => b.Ratings)
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(x => x.Id == Id);

            return _mapper.Map<Book, BookDetailsDTO>(book);
        }

        /// <summary>
        /// Check if book is exist
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> BookExistAsync(int Id)
        {
            return await _context.Books.AnyAsync(b => b.Id == Id);
        }

        /// <summary>
        /// Delete book
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteBookAsync(int Id)
        {
            var book = await _context.Books.FindAsync(Id);
            if (book != null)
            {
                var reviews = _context.Reviews.Where(x => x.BookId == Id);
                _context.Reviews.RemoveRange(reviews);

                var reatings = _context.Ratings.Where(x => x.BookId == Id);
                _context.Ratings.RemoveRange(reatings);

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Save book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Book Id</returns>
        public async Task<int> SaveBookAsync(BookRequestDTO book)
        {
            var book_= _mapper.Map<BookRequestDTO, Book>(book);

            if (!await BookExistAsync(book.Id))
                _context.Books.Add(book_);
            else
                _context.Books.Update(book_);
            
            await _context.SaveChangesAsync();
            return book_.Id;
        }

        /// <summary>
        /// Save review
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="review"></param>
        /// <returns>Review Id</returns>
        public async Task<int> SaveReviewAsync(int Id, ReviewRequestDTO review)
        {
            var book = await _context.Books
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == Id);

            var review_ = _mapper.Map<ReviewRequestDTO, Review>(review);
            review_.BookId = Id;

            _context.Reviews.Add(review_);
            await _context.SaveChangesAsync();

            return review_.Id;
        }

        /// <summary>
        /// Rate book
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public async Task RateBookAsync(int Id, RatingRequestDTO rating)
        {
            var book = await _context.Books
                .Include(x => x.Ratings)
                .FirstOrDefaultAsync(x => x.Id == Id);

            var rating_ = _mapper.Map<RatingRequestDTO, Rating>(rating);
            rating_.BookId = Id;

            _context.Ratings.Add(rating_);
            await _context.SaveChangesAsync();          
        }
    }
}
