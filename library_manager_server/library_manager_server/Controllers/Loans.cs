
using System.Security.Claims;
using library_manager_server.Controllers;
using library_manager_server.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace library_manager_server;

[Authorize(policy: "ActiveSession")]
[ApiController]
[Route("api/[controller]")]
public class Loans(ILibraryManager libraryManager, ILogger<Loans> logger, ISessionHandler sessionHandler) : ControllerBase
{
    private readonly ISessionHandler _sessionHandler = sessionHandler;
    private readonly ILibraryManager _libraryManager = libraryManager;
    private readonly ILogger<Loans> _logger = logger;
    
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<Loan>>> GetLoans()
    {   
        _logger.LogInformation("GetLoans called");
        string? sessionId = _sessionHandler.GetSession(HttpContext);
        if(sessionId == null) return Unauthorized();
        double? userId = _sessionHandler.GetUserId(sessionId);
        if(userId == null) return Unauthorized();
        return _libraryManager.GetLoans(userId.Value);
    }
    
    [HttpGet("{loanId:double}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Loan>> GetLoan(double loanId)
    {
        _logger.LogInformation("GetLoan called");
        string? providedSessionId = _sessionHandler.GetSession(HttpContext);
        if (providedSessionId == null)
        {
            _logger.LogDebug("No session found");
            return Unauthorized();
        }
        double? userIdOrnull = _sessionHandler.GetUserId(providedSessionId);
        if (userIdOrnull == null)
        {
            _logger.LogDebug("No user associated with this session");
            return Unauthorized();
        } 
        if(!_libraryManager.OwnsLoan(userIdOrnull.Value, loanId)) return Forbid();
        Loan? loan = _libraryManager.GetLoan(loanId);
        if (loan == null)
        {
            _logger.LogDebug("No loan associated with this loanId");
            return BadRequest();
        }
        return loan;
    }

    [HttpPost("loan/{isbn}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLoan(string isbn)
    {
        _logger.LogInformation("Creating loan {isbn}", isbn);
        string? sessionId = _sessionHandler.GetSession(HttpContext);
        if(sessionId == null) return Unauthorized();
        double? userIdOrnull = _sessionHandler.GetUserId(sessionId);
        if (userIdOrnull == null){ return Unauthorized(); } 
        double userid = userIdOrnull.Value;
        if (_libraryManager.HasActiveLoan(isbn))
        {
            _logger.LogInformation($"Loan {isbn} already active");
            return Forbid();
        }
        bool loanCreated = _libraryManager.CreateLoan(isbn, userid, DateTime.Now);
        if (loanCreated)
        {
            _logger.LogInformation($"Created loan {isbn}");
            return Created();
        }
        return BadRequest();
    }

    [HttpDelete("loan/{loanId:double}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteLoans(double loanId)
    {
        _logger.LogInformation("Deleting loan {loanId}", loanId);
        string? sessionId = _sessionHandler.GetSession(HttpContext);
        if(sessionId == null) return Unauthorized();
        double? userId = _sessionHandler.GetUserId(sessionId);
        if (userId == null){ return Unauthorized(); } 
        bool ret = _libraryManager.OwnsLoan(loanId, userId.Value);
        if(ret == false) return Forbid();
        bool didDelete = _libraryManager.DeleteLoan(loanId);
        if(!didDelete) return BadRequest();
        return Ok();
    }
}