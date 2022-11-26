using VacationAPI.Context;
using VacationAPI.Services;
using VacationAPI.Services.Validation;

namespace VacationAPI.Request.Authentication.Post;

public class User
{
	//Добавление нового пользователя в базу данных
	public static IResult AddNewUser(ApplicationContext db, UserValidator userValidator, ILogger logger, string username, string password)
	{
		logger.LogInformation("Add new user: start");

		//проверка на валидность введенного имени пользователя
		if (!userValidator.Validate(new Services.Validation.User(username))
				.IsValid)
		{
			logger.LogError("Add new user: Wrong format of input");

			//Ответ в случае ошибки
			return Results.Json(
				"Недопустимый формат ввода. Имя пользователя должно состоять только из букв и не превышать длину в 50 символов");
		}

		//Проверка на наличие ползователя с таким же именем в базе данных
		if (!db.Users.Any(x => x.Name == username))
		{
			//Создание нового пользователя
			db.Users.Add(new()
			{
				Name = username,
				Password = MD5Hash.GetHashedString(password), //хеширование пароля
				Teams = new()
			});

			db.SaveChanges();

			//Ответ пользователю в случае успеха. Содержит имя и пароль пользователя
			var response = new
			{
				username,
				password
			};

			logger.LogInformation("Add new user: successfully");

			return Results.Json(response);
		}

		logger.LogError("Request error: user with this name already exists");

		//Ответ в случае ошибки.
		return Results.Json("Пользователь с таким ником уже существует");
	}
}