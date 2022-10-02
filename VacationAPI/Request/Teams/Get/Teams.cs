using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Teams.Get;

public class Teams
{
	public static IResult GetTeams(ApplicationContext db, string username, string accessToken)
	{
		if (db.Users.FirstOrDefault(x => x.Name == username && x.Teams.Any()) != null)
		{
			User user = db.Users.FirstOrDefault(x => x.Name == username);
			string teams = string.Join(", ", user.Teams);

			return Results.Json(teams);
		}

		return Results.Json("Список команд пуст");
	}
}