using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Teams.Post;

public class NewTeam
{
	public static IResult AddNewTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		if (db.Users.FirstOrDefault(x => x.Name == username) != null && db.Teams.FirstOrDefault(x => x.Name == teamName) == null)
		{
			db.Teams.Add(new()
			{
				User = db.Users.FirstOrDefault(x => x.Name == username),
				Name = teamName
			});

			db.SaveChanges();

			var response = new
			{
				username,
				teamName
			};

			return Results.Json(response);
		}

		return Results.Json("Команда с таким именем уже существует");
	}
}