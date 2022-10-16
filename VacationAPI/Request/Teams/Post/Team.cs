using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Post;

public class Team
{
	public static IResult AddNewTeam(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Add new team: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, newTeamName: teamName);
		User user = db.Users.FirstOrDefault(x => x.Name == username)!;

		if (request == null)
		{
			db.Teams.Add(new()
			{
				User = user,
				Name = teamName
			});

			db.SaveChanges();

			var response = new
			{
				username,
				teamName
			};

			logger.LogInformation("Add new team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Add new team: failed");

		return request;
	}
}