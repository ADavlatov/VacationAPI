using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Get;

public class Teams
{
	public static IResult? GetTeams(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		logger.LogInformation("Get teams: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken);
		User user = db.Users.FirstOrDefault(x => x.Name == username);

		if (db.Teams.FirstOrDefault(x => x.User == user) == null)
		{
			logger.LogInformation("Get teams: successfully");

			Results.Json("Список команд пуст");
		}

		if (user != null && request == null)
		{
			var userTeams = from team in db.Teams
							where team.User == user
							select team.Name;

			string teams = string.Join(", ", userTeams);

			logger.LogInformation("Get teams: successfully");

			return Results.Json(teams);
		}

		logger.LogError("Get teams: failed");

		return request;
	}
}