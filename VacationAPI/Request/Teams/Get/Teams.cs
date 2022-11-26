using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Teams.Get;

public class Teams
{
	public static IResult GetTeams(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			List<string> teamsName = new();

			foreach (var team in db.Teams)
			{
				if (team.User.Name == username)
				{
					teamsName.Add(team.Name);
				}
			}

			string teams = string.Join(", ", teamsName);

			return Results.Json(teams);
		}

		return result;
	}
}