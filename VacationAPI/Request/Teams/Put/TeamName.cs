using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Teams.Put;

public class TeamName
{
	public static IResult EditTeamName(ApplicationContext db, string teamName, string newTeamName, string username, string accessToken)
	{
		if (db.Users.FirstOrDefault(x => x.Name == username && x.Teams.FirstOrDefault(x => x.Name == newTeamName) == null) != null)
		{
			var team = db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName);
			team.Name = newTeamName;

			var response = new
			{
				oldTeamName = teamName,
				newTeamName
			};

			return Results.Json(response);
		}

		return Results.Json("Команда с таким именем уже существует");
	}
}