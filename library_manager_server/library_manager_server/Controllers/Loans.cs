
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server;

[ApiController]
[Route("api/[controller]")]
public class Loans : ControllerBase
{
    LibraryManager libraryManager;
    public Loans(LibraryManager libraryManager) => this.libraryManager = libraryManager;

    [HttpGet()]
    public async Task<IActionResult> GetLoans()
    {
        return NotFound();
    }

    [HttpPost()]
    public async Task<IActionResult> CreateLoan()
    {
        
        return NotFound();
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteLoans()
    {
        
        return NotFound();
    }
        
    
    
}