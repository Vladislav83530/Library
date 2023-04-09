using AutoMapper;
using Library.API.Controllers;
using Library.BLL.DTOs;
using Library.BLL.Services.Interfaces;
using Library.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Library.UnitTests
{
    public class LibraryControllerTests
    {
        private readonly Mock<ILibraryService> _mockLibraryService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LibraryController _controller;

        public LibraryControllerTests()
        {
            _mockLibraryService = new Mock<ILibraryService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();
            _controller = new LibraryController(_mockLibraryService.Object, _mockConfiguration.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOk()
        {
            var books = new List<Book>();
            _mockLibraryService
                .Setup(s => s.GetAllBooksAsync(null))
                .ReturnsAsync(books);

            var bookDTOs = new List<BookDTO>();
            _mockMapper
                .Setup(m => m.Map<IEnumerable<Book>, List<BookDTO>>(books))
                .Returns(bookDTOs);

            var result = await _controller.GetAllBooks(null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(okResult.Value);
            Assert.Equal(bookDTOs, model);
        }

        [Fact]
        public async Task GetRecommendedBooks_ReturnsOk()
        {
            var recommendBooks = new List<BookDTO>();
            _mockLibraryService
                .Setup(s => s.GetRecommendedBooksAsync(null))
                .ReturnsAsync(recommendBooks);

            var result = await _controller.GetRecommendedBooks(null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(okResult.Value);
            Assert.Equal(recommendBooks, model);
        }

        [Fact]
        public async Task GetBookDetails_WithExistingBookId_ReturnsOk()
        {
            var book = new BookDetailsDTO();

            _mockLibraryService
                .Setup(s => s.BookExistAsync(1))
                .ReturnsAsync(true);
            _mockLibraryService
                .Setup(s => s.GetBookDetailsAsync(1))
                .ReturnsAsync(book);

            var result = await _controller.GetBookDetails(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BookDetailsDTO>(okResult.Value);
            Assert.Equal(book, model);
        }

        [Fact]
        public async Task GetBookDetails_WithNonExistingBookId_ReturnsNotFound()
        {
            _mockLibraryService
                .Setup(s => s.BookExistAsync(1))
                .ReturnsAsync(false);

            var result = await _controller.GetBookDetails(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBook_WithValidSecretAndExistingBook_ReturnsOk()
        {
            int bookId = 1;
            string secretKey = "mySecretKey";
            _mockConfiguration.Setup(x => x["SecretKey"]).Returns(secretKey);
            _mockLibraryService.Setup(x => x.BookExistAsync(bookId)).ReturnsAsync(true);

            var result = await _controller.DeleteBook(bookId, secretKey);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteBook_WithInvalidSecret_ReturnsBadRequest()
        {

            int bookId = 1;
            string invalidSecret = "invalidSecretKey";
            _mockConfiguration.Setup(x => x["SecretKey"]).Returns("mySecretKey");

            var result = await _controller.DeleteBook(bookId, invalidSecret);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid secret key", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteBook_WithNonExistingBook_ReturnsNotFound()
        {

            int nonExistingBookId = 1;
            string secretKey = "mySecretKey";
            _mockConfiguration
                .Setup(x => x["SecretKey"])
                .Returns(secretKey);

            _mockLibraryService
                .Setup(x => x.BookExistAsync(nonExistingBookId))
                .ReturnsAsync(false);


            var result = await _controller.DeleteBook(nonExistingBookId, secretKey);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SaveBook_ReturnsOk_WhenModelIsValid()
        {
            var bookRequestDTO = new BookRequestDTO
            {
                Id = 1,
                Title = "Book Title",
                Cover = "base64EncodedString",
                Content = "Book content",
                Genre = "Fantasy",
                Author = "John Doe"
            };
            var expectedResult = new BookRequestDTO { Id = 1 };

            _mockLibraryService
                .Setup(s => s.SaveBookAsync(bookRequestDTO))
                .ReturnsAsync(1);

            var result = await _controller.SaveBook(bookRequestDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<int>(okResult.Value);
            Assert.Equal(expectedResult.Id, actualResult);
        }

        [Fact]
        public async Task SaveReview_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var reviewRequestDTO = new ReviewRequestDTO { Message = "Book review", Reviewer = "Jane Doe" };

            _mockLibraryService.Setup(s => s.BookExistAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _controller.SaveReview(1, reviewRequestDTO);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RateBook_ReturnsOk_WhenModelIsValid()
        {
            var ratingRequestDTO = new RatingRequestDTO { Score = 4 };

            _mockLibraryService.Setup(s => s.BookExistAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _controller.RateBook(1, ratingRequestDTO);

            Assert.IsType<OkResult>(result);
        }
    }
}