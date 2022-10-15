using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Get;

public class Teams
{
	public static IResult? GetTeams(ApplicationContext db, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken);
		User user = db.Users.FirstOrDefault(x => x.Name == username);

		if (db.Teams.FirstOrDefault(x => x.User == user) == null)
		{
			Results.Json("Список команд пуст");
		}
		if (user != null && request == null)
		{
			var userTeams = from team in db.Teams
							where team.User == user
							select team.Name;
			string teams = string.Join(", ", userTeams);

			return Results.Json(teams);
		}

		return request;
	}
}