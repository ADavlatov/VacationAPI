using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Authentication.Post;

public class User
{
	public static IResult AddNewUser(ApplicationContext db, ILogger logger, string username, string password)
	{
		logger.LogInformation("Add new user: start");

		if (db.Users.FirstOrDefault(x => x.Name == username) == null)
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
		logger.LogError("Add new user: failed");

		return Results.Json("Пользователь с таким ником уже существует");
	}
}