using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Put;

public class TeamName
{
	public static IResult EditTeamName(ApplicationContext db, string teamName, string newTeamName, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, newTeamName: newTeamName);
		Team team = db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName && x.Name != newTeamName);

		if (team != null && request == null)
		{
			team.Name = newTeamName;
			db.SaveChanges();

			var response = new
			{
				oldTeamName = teamName,
				newTeamName
			};

			return Results.Json(response);
		}

		return request;
	}
}