using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace library_manager_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authentication : ControllerBase
	{
		public static readonly string SESSION_ID_NAME = "sessionId";

        private LibrayManager libraryManger;

        public Authentication(LibrayManager librayManager) {
			this.libraryManger = librayManager;
		}

		//public IActionResult Register()
		//{
		//	return NotFound();
		//}

		[HttpPost()]
		public async Task<IActionResult> Login( string username,  string password) { 

			switch (libraryManger.AuthenticateUser(username, password))
			{
				case Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success:
					Console.WriteLine("sdojasfjasnf");
					Guid guid = Guid.NewGuid();
					double? userId = libraryManger.GetUserId(username);
					if(userId is not null)
					{
						libraryManger.AddSession(userId.Value, guid);

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
		[HttpGet("/test")]
		public async Task<string> testc()
		{
			return "hello world";
		}
	}
}
