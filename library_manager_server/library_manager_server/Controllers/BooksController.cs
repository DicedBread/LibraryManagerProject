using library_manager_server.ClientContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        ILogger<BooksController> _log;
        private readonly ILibraryManager _libraryManager;

        public BooksController(ILibraryManager libraryManager, ILogger<BooksController> log)
        {
            this._libraryManager = libraryManager;
            _log = log;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ClientContext.Book>> GetBooks([FromQuery] string? search, [FromQuery] int limit = 20,[FromQuery] int offset = 0)
        {
            List<ClientContext.Book> books;
            if (limit <= 0 || offset < 0) return BadRequest();
            if (search == null)
            {
                books = _libraryManager.GetBooks(limit, offset);
            }
            else
            {
                _log.LogInformation($"Searching for {search} of {limit} books.");
                books = _libraryManager.SearchBooks(search, limit, offset);
            }
            return books.ToArray();
        }
        
        [HttpGet("{isbn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ClientContext.Book> GetBook(string isbn)
        {
            ClientContext.Book? book = _libraryManager.GetBook(isbn);
            if(book == null) return NotFound();
            return book;
        }
    }
}
