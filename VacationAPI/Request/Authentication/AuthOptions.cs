using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VacationAPI.Request.Authentication;

public class AuthOptions
{
	public const string Issuer = "Server";
	public const string Audience = "Client";
	const string Key = "mysupersecret_secretkey!123";
	public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
		new (Encoding.UTF8.GetBytes(Key));
}