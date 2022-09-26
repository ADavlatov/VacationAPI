using VacationAPI.Authentication;
using VacationAPI.Context;

namespace VacationAPI.Queries.Employees.Post;

public class NewTeam
{
	public static IResult AddNewTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		if (JwtToken.CheckJwtToken(username, accessToken) && db.Users.FirstOrDefault(x => x.Name == username) != null && db.Teams.FirstOrDefault(x => x.Name == teamName) == null)
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
		if (!JwtToken.CheckJwtToken(username, accessToken))
		{
			Results.Json("Неверный access_token");
		}

		return Results.Json("Команда с таким именем уже существует");
	}
}