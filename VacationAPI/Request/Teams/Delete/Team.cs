using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Teams.Delete;

public class Team
{
	public static IResult RemoveTeam(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Delete team: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);
		Entities.Team team = db.Teams.FirstOrDefault(x => x.Name == teamName)!;

		if (request == null)
		{
			db.Teams.Remove(team);
			db.SaveChanges();

			logger.LogInformation("Delete team: successfully");

			return Results.Json($"Команда {teamName} удалена");
		}

		logger.LogError("Delete team: failed");

		return request;
	}
}