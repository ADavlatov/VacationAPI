using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using VacationAPI.Authentication;
using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services;

namespace VacationAPI.Request.Authentication.Get;

public class JwtToken
{
	public static IResult GetJwtToken(ApplicationContext db, string username, string password)
	{
		if (GetClaims(db, username, MD5Hash.GetHashedString(password)).Claims != null)
		{
			var response = new
			{
				access_token = new JwtSecurityTokenHandler().WriteToken(GenerateToken(db, username, MD5Hash.GetHashedString(password))),
			};

			return Results.Json(response);
		}

		return Results.Json("Неверный логин или пароль");
	}

	private static JwtSecurityToken GenerateToken(ApplicationContext db, string username, string password)
	{
		return new(issuer: AuthOptions.ISSUER,
			audience: AuthOptions.AUDIENCE,
			claims: GetClaims(db, username, password)
				.Claims,
			expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
			signingCredentials: new(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
	}

	private static ClaimsIdentity GetClaims(ApplicationContext db, string username, string password)
	{
		User user = db.Users.FirstOrDefault(x => x.Name == username && x.Password == password);

		if (user != null)
		{
			var claims = new List<Claim>
			{
				new(ClaimsIdentity.DefaultNameClaimType, user.Name)
			};

			ClaimsIdentity claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);

			return claimsIdentity;
		}

		return null;
	}
}