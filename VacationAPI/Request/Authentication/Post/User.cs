using VacationAPI.Context;
using VacationAPI.Services;

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
				Password = MD5Hash.GetHashedString(password)
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