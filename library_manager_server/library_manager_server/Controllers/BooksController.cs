using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private LibrayManager LibrayManager;

        public BooksController(LibrayManager libraryManager)
        {
            LibrayManager = libraryManager;
        }

        // [HttpGet("/allbooks")]
        // public IEnumerable<Book> GetAllBooks()
        // {
        //     List<Book> output = LibrayManager.GetBooks();
        //     return output.ToArray();
        // }

        [HttpGet()]
        public ActionResult<IEnumerable<Book>> GetBooks([FromQuery] int limit,[FromQuery] int offset)
        {
            if (limit <= 0 || offset < 0) return BadRequest();
            List<Book> books = LibrayManager.GetBooks(limit, offset);
            return books.ToArray();
        }

        
        [HttpGet("{isbn}")]
        public ActionResult<Book> GetBook(int isbn)
        {
            Book? book = LibrayManager.GetBook(isbn);
            if(book == null) return NotFound();
            return book;
        }
    }
}
