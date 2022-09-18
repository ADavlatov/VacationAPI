using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using VacationAPI.Context;
using VacationAPI.Entities;

namespace VacationAPI.Authentication;

public class JwtToken
{
	public static JwtSecurityToken GenerateToken(ApplicationContext db, string username, string password)
	{
		return new (
			issuer: AuthOptions.ISSUER,
			audience: AuthOptions.AUDIENCE,
			claims: GetClaims(db, username, password).Claims,
			expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
			signingCredentials: new (AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
	}

	public static bool CheckJwtToken(string username, string jwtToken)
	{
		JwtSecurityToken jwt = new JwtSecurityToken(jwtToken);
		if (username == jwt.Claims.First().Value)
		{
			return true;
		}

		return false;
	}
	private static ClaimsIdentity GetClaims(ApplicationContext db, string username, string password)
	{
		User user = db.Users.FirstOrDefault(x => x.Name == username && x.Password == password);

		if (user != null)
		{
			var claims = new List<Claim>
			{
				new (ClaimsIdentity.DefaultNameClaimType, user.Name),
			};
			ClaimsIdentity claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);
			return claimsIdentity;
		}

		return null;
	}
}