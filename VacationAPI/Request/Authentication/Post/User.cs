using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Authentication.Post;

public class User
{
	public static IResult AddNewUser(ApplicationContext db, string username, string password)
	{
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

			return Results.Json(response);
		}

		return Results.Json("Пользователь с таким ником уже существует");
	}
}