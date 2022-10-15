using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Delete;

public class Team
{
	public static IResult RemoveTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName);
		Entities.Team team = db.Teams.FirstOrDefault(x => x.Name == teamName);

		if (team != null && request == null)
		{
			db.Teams.Remove(team);
			db.SaveChanges();

			return Results.Json($"Команда {teamName} удалена");
		}

		return request;
	}
}