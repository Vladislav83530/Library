using Library.BLL.DTOs;

namespace Library.BLL.Services.Interfaces
{
    public interface ILibraryService
    {
        public Task<IEnumerable<BookDTO>> GetAllBooksAsync(string order);
        public Task<IEnumerable<BookDTO>> GetRecommendedBooksAsync(string genre);
        public Task<BookDetailsDTO> GetBookDetailsAsync(int Id);
        public Task<bool> BookExistAsync(int Id);
        public Task DeleteBookAsync(int Id);
        public Task<int> SaveBookAsync(BookRequestDTO book);
        public Task<int> SaveReviewAsync(int Id, ReviewRequestDTO review);
        public Task RateBookAsync(int id, RatingRequestDTO rating);
    }
}
