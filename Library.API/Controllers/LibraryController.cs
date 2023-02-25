using AutoMapper;
using Library.BLL.DTOs;
using Library.BLL.Services.Interfaces;
using Library.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public LibraryController(ILibraryService libraryService, IConfiguration configuration, IMapper mapper)
        {
            _libraryService = libraryService;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all books Order by provided value (title or author)
        /// GET https://{{baseUrl}}/api/books?order=author
        /// </summary>
        /// <param name="order"></param>
        /// <returns>
        /// Response
        /// [{
        ///     "id": "number",
        ///     "title": "string",
        ///     "author": "string",
        ///     "rating": "decimal",            average rating
        ///     "reviewsNumber": "number"    	count of reviews
        /// }]
        ///</returns>
        [HttpGet]
        [Route("books")]
        public async Task<IActionResult> GetAllBooks([FromQuery] string? order)
        {
            var books = await _libraryService.GetAllBooksAsync(order);
            var books_ = _mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
            return Ok(books_);
        }

        /// <summary>
        ///  Get top 10 books with high rating and number of reviews greater than 10
        ///  Filter books by specifying genre. 
        ///  Order by rating       
        ///  GET https://{{baseUrl}}/api/recommended?genre=horror
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>
        /// Response
        /// [{
        /// 	"id": "number",
        ///     "title": "string",
        ///	    "author": "string",
        /// 	"rating": "decimal",          	average rating
        /// 	"reviewsNumber": "number"    	count of reviews
        /// }]
        /// </returns>
        [HttpGet]
        [Route("recommended")]
        public async Task<IActionResult> GetRecommendedBooks([FromQuery] string? genre)
        {
            var recommendBooks = await _libraryService.GetRecommendedBooksAsync(genre);
            return Ok(recommendBooks);
        }

        /// <summary>
        /// Get book details with the list of reviews
        /// GET https://{{baseUrl}}/api/books/{id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        /// Response
        /// {
        ///	    "id": "number",
        ///	    "title": "string",
        /// 	"author": "string",
        /// 	"cover": "string",
        /// 	"content": "string",
        /// 	"rating": "decimal",        average rating
        /// 	"reviews": [{
        ///     	    "id": "number",
        ///     	    "message": "string",
        ///     	    "reviewer": "string",
        /// 	}]
        /// }}
        /// </returns>
        [HttpGet]
        [Route("books/{id}")]
        public async Task<IActionResult> GetBookDetails(int id)
        {
            if (!await _libraryService.BookExistAsync(id))
                return NotFound();

            var result = await _libraryService.GetBookDetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        ///  Delete a book using a secret key
        ///  DELETE https://{{baseUrl}}/api/books/{id}?secret=qwerty
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("books/{id}")]
        public async Task<IActionResult> DeleteBook(int id, [FromQuery] string secret)
        {
            if (secret != _configuration["SecretKey"])
                return BadRequest("Invalid secret key");

            if (!await _libraryService.BookExistAsync(id))
                return NotFound();

            await _libraryService.DeleteBookAsync(id);
            return Ok();
        }

        /// <summary>
        /// Save a new book
        /// POST https://{{baseUrl}}/api/books/save
        /// {
	    ///     "id": "number",             	
	    ///     ": "string",
	    ///     "cover": "string",        save image as base64
	    ///     "content": "string",
	    ///     "genre": "string",
	    ///     "author": "string"
        /// }
        /// </summary>
        /// <param name="book"></param>
        /// <returns>
        /// Response
        /// Id
        /// </returns>
        [HttpPost]
        [Route("books/save")]
        public async Task<IActionResult> SaveBook([FromBody] BookRequestDTO book)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _libraryService.SaveBookAsync(book);

            return Ok(result);
        }

        /// <summary>
        /// Save a review for the book.
        /// PUT https://{{baseUrl}}/api/books/{id}/review
        /// {
	    ///     "message": "string",
	    ///     "reviewer": "string",
        /// }
        /// </summary>
        /// <param name="id"></param>
        /// <param name="review"></param>
        /// <returns>
        /// Response
        /// Id
        /// </returns>
        [HttpPut]
        [Route("books/{id}/review")]
        public async Task<IActionResult> SaveReview(int id, [FromBody] ReviewRequestDTO review)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _libraryService.BookExistAsync(id))
                return NotFound();

            var result = await _libraryService.SaveReviewAsync(id, review);
            return Ok(result);
        }

        /// <summary>
        /// Rate a book
        /// PUT https://{{baseUrl}}/api/books/{id}/rate
        /// {
	    ///     "score": "number",    score can be from 1 to 5
        /// }
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("books/{id}/rate")]
        public async Task<IActionResult> RateBook(int id, [FromBody] RatingRequestDTO rating)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _libraryService.BookExistAsync(id))
                return NotFound();

            await _libraryService.RateBookAsync(id, rating);
            return Ok();
        }
    }
}
