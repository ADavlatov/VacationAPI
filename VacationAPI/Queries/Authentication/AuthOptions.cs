using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VacationAPI.Authentication;

public class AuthOptions
{
	public const string ISSUER = "Server";
	public const string AUDIENCE = "Client";
	const string KEY = "mysupersecret_secretkey!123";
	public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
		new (Encoding.UTF8.GetBytes(KEY));
}