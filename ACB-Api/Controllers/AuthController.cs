using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ACB_Api.Controllers {
	[ApiController]
	[Route("acb-api")]
	public class AuthController : ControllerBase {
		[HttpPost("token")]
		public IActionResult GetToken () {
			// Aquí deberías validar las credenciales del usuario

			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, "userId"),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ReallyDifficultPasswordToGetTheToken123")); // Usa la misma clave secreta que en la configuración
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: "acb-api",
				audience: "acb-api",
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			return Ok(new {
				token = new JwtSecurityTokenHandler().WriteToken(token)
			});
		}
	}

}
