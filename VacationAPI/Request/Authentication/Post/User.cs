using VacationAPI.Context;
using VacationAPI.Services;
using VacationAPI.Services.Validation;

namespace VacationAPI.Request.Authentication.Post;

public class User
{
	public static IResult AddNewUser(ApplicationContext db, UserValidator userValidator, ILogger logger, string username, string password)
	{
		logger.LogInformation("Add new user: start");

		if (!userValidator.Validate(new Services.Validation.User(username))
				.IsValid)
		{
			logger.LogError("Add new user: Wrong format of input");

			return Results.Json(
				"Недопустимый формат ввода. Имя пользователя должно состоять только из букв и не превышать длину в 50 символов");
		}

		if (!db.Users.Any(x => x.Name == username))
		{
			db.Users.Add(new()
			{
				Name = username,
				Password = MD5Hash.GetHashedString(password),
				Teams = new()
			});

			db.SaveChanges();

			var response = new
			{
				username,
				password
			};

			logger.LogInformation("Add new user: successfully");

			return Results.Json(response);
		}

		logger.LogError("Request error: user with this name already exists");

		return Results.Json("Пользователь с таким ником уже существует");
	}
}