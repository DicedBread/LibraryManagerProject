using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace library_manager_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authentication : ControllerBase
	{
        private LibrayManager libraryManger;

        public Authentication(LibrayManager librayManager) {
			this.libraryManger = librayManager;
		}

		//public IActionResult Register()
		//{
		//	return NotFound();
		//}

		[HttpPost()]
		public async Task<IActionResult> Login([FromHeaderAttribute] string username, [FromHeaderAttribute] string password) { 

			

			if(username == "test" && password == "test"){
				var claims = new List<Claim>() {
					new Claim("username", username)
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
			else
			{
				return Unauthorized();
			}

			return NotFound();
		}
	}
}
