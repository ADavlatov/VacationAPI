using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using VacationAPI.Context;

namespace VacationAPI.Validation;

public class RequestValidator : AbstractValidator<Entities.Request>
{
	public RequestValidator(ApplicationContext db)
	{
		var msgUsername = "Ошибка в поле username: пользователь не найден";
		var msgAccessToken = "Ошибка в поле access_token: неверный access_token";

		RuleFor(x => x.Username)
			.Must(x => db.Users.FirstOrDefault(j => j.Name == x) != null)
			.WithMessage(msgUsername);

		RuleFor(x => x.AccessToken)
			.Must(x => db.Users.FirstOrDefault(j => j.Name
													== new JwtSecurityToken(x).Claims.First()
														.Value)
						!= null)
			.WithMessage(msgAccessToken);
	}
}