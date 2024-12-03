
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server;

[ApiController]
[Route("api/[controller]")]
public class Loans(LibraryManager libraryManager) : ControllerBase
{
    LibraryManager libraryManager = libraryManager;

    [HttpGet()]
    public async Task<IActionResult> GetLoans()
    {
        return NotFound();
    }
    
    [HttpGet("{loanId:double}")]
    public async Task<IActionResult> GetLoan(double loanId)
    {       
        return NotFound();
    }

    [HttpPost("/loan/{loanId:double}")]
    public async Task<IActionResult> CreateLoan(double loanId)
    {
        
        return NotFound();
    }

    [HttpDelete("/loan/{loanId:double}")]
    public async Task<IActionResult> DeleteLoans(double loanId)
    {
        
        return NotFound();
    }
        
    
    
}