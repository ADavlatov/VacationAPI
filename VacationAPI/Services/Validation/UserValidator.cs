using FluentValidation;

namespace VacationAPI.Services.Validation;

public class UserValidator : AbstractValidator<User>
{
	public UserValidator()
	{
		RuleFor(x => x.Username)
			.Must(x => x.All(char.IsLetter))
			.Must(x => x.Length < 50);
	}
}

public class User
{
	public User(string username)
	{
		Username = username;
	}

	public string Username { get; }
}