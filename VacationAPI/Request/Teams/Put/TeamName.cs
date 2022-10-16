using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Put;

public class TeamName
{
	public static IResult EditTeamName(ApplicationContext db, ILogger logger, string teamName, string newTeamName, string username, string accessToken)
	{
		logger.LogInformation("Change team name: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, newTeamName: newTeamName);
		Team team = db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName && x.Name != newTeamName)!;

		if (request == null)
		{
			team.Name = newTeamName;
			db.SaveChanges();

			var response = new
			{
				oldTeamName = teamName,
				newTeamName
			};

			logger.LogInformation("Change team name: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change team name: failed");

		return request;
	}
}