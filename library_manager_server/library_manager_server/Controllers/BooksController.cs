using library_manager_server.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private LibrayManager librayManager;

        public BooksController(LibrayManager libraryManager) => this.librayManager = libraryManager;

        [HttpGet()]
        public ActionResult<IEnumerable<Book>> GetBooks([FromQuery] int limit,[FromQuery] int offset)
        {
            if (limit <= 0 || offset < 0) return BadRequest();
            List<Book> books = librayManager.GetBooks(limit, offset);
            return books.ToArray();
        }
        
        [HttpGet("{isbn}")]
        public ActionResult<Book> GetBook(string isbn)
        {
            Book? book = librayManager.GetBook(isbn);
            if(book == null) return NotFound();
            return book;
        }
    }
}
