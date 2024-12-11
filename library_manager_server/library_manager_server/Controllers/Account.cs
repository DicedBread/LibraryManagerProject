using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Claims;

namespace library_manager_server.Controllers
{
	[Authorize(policy: "ActiveSession")]
    [ApiController]
    [Route("api/[controller]")]
    public class Account : ControllerBase
	{
		public static readonly string SESSION_ID_NAME = "sessionId";

        private readonly ILibraryManager _libraryManger;
        private readonly ISessionHandler _sessionHandler;
        private readonly ILogger<Account> _logger;

        public Account(ILibraryManager libraryManager, ISessionHandler sessionHandler, ILogger<Account> logger) {
			this._libraryManger = libraryManager;
            this._sessionHandler = sessionHandler;
            this._logger = logger;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register(string email, string password, string username)
		{
			_logger.LogInformation($"regiter of email {email} attempt");
			email = email.Trim();
			password = password.Trim();	
			username = username.Trim();

			if(!IsValidEmail(email)) { BadRequest("Invalid email"); }
			if(!IsValidPassword(password)) { BadRequest("Invalid password"); }
			
			_logger.LogInformation("Register attempted");
			if (_libraryManger.AddUser(email, password, username))
			{
				_logger.LogInformation($"Register of {email} successfully");
				return Ok();
			}
			_logger.LogInformation($"Register failed {email}");
			return BadRequest();
		}

        private bool IsValidPassword(string password)
        {
			// TODO add password req
			return true;
        }

        private bool IsValidEmail(string email)
		{
			if (email.EndsWith(".")) return false;
			try
			{
				MailAddress addr = new MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}


		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(string email, string password) {
			_logger.LogInformation("login attempted");
			switch (_libraryManger.AuthenticateUser(email, password))
			{
				case Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success:
					_logger.LogInformation($"Successfull authenticated user: {email} ");
					Guid guid = Guid.NewGuid();
					double? userId = _libraryManger.GetUserId(email);
					if(userId is not null)
					{
						_sessionHandler.AddSession(userId.Value, guid);

						List<Claim> claims = new List<Claim>() {
							new Claim(SESSION_ID_NAME, guid.ToString()),
						};

                        ClaimsIdentity claimsIden = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authProperties = new AuthenticationProperties();
                        // TODO Configure claims

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIden),
                        authProperties
                    );
                        return Ok();
					}
					return Unauthorized();


                case Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed:
					_logger.LogInformation($"Failed to authenticate user: {email}");
					return Unauthorized();
				default: return Unauthorized();
			}
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			_logger.LogInformation("logout");
			string providedSessionId = HttpContext.User.Claims.First(c => c.Type == Account.SESSION_ID_NAME).Value.ToString();
			if (_sessionHandler.RemoveSession(providedSessionId))
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Ok();
			};
			return BadRequest();
		}

		[Authorize(Policy = "ActiveSession")]
		[HttpGet("test")]
		public async Task<string> test()
		{
			return "hello world";
		}
	}
}
