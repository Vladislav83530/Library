using Library.BLL.DTOs;
using Library.DAL.Entities;

namespace Library.BLL.Services.Interfaces
{
    public interface ILibraryService
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync(string? order);
        public Task<IEnumerable<BookDTO>> GetRecommendedBooksAsync(string? genre);
        public Task<BookDetailsDTO> GetBookDetailsAsync(int Id);
        public Task<bool> BookExistAsync(int? Id);
        public Task DeleteBookAsync(int Id);
        public Task<int> SaveBookAsync(BookRequestDTO book);
        public Task<int> SaveReviewAsync(int Id, ReviewRequestDTO review);
        public Task RateBookAsync(int id, RatingRequestDTO rating);
    }
}
