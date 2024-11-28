using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace library_manager_server.Controllers
{
	[Authorize(policy: "ActiveSession")]
    [ApiController]
    [Route("api/[controller]")]
    public class Account : ControllerBase
	{
		public static readonly string SESSION_ID_NAME = "sessionId";

        private LibrayManager libraryManger;
        private readonly SessionHandler sessionHandler;
        private readonly ILogger<Account> logger;

        public Account(LibrayManager librayManager, SessionHandler sessionHandler, ILogger<Account> logger) {
			this.libraryManger = librayManager;
            this.sessionHandler = sessionHandler;
            this.logger = logger;
        }

		//public IActionResult Register()
		//{
		//	return NotFound();
		//}
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login(string username, string password) {
			logger.LogInformation("login attempted");
			switch (libraryManger.AuthenticateUser(username, password))
			{
				case Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success:
					Guid guid = Guid.NewGuid();
					double? userId = libraryManger.GetUserId(username);
					if(userId is not null)
					{
						sessionHandler.AddSession(userId.Value, guid);

						var claims = new List<Claim>() {
							new Claim(SESSION_ID_NAME, guid.ToString()),
						};


                        ClaimsIdentity claimsIden = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIden),
                        authProperties
                    );
                        return Ok();
					}
					return Unauthorized();


                case Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed:
					return Unauthorized();
				

				default: return Unauthorized();
			}
		}

		[Authorize(Policy = "ActiveSession")]
		[HttpGet("test")]
		public async Task<string> test()
		{
			return "hello world";
		}
	}
}
