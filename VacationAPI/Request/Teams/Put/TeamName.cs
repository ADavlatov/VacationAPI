using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using Team = VacationAPI.Entities.Team;

namespace VacationAPI.Request.Teams.Put;

public class TeamName
{
	public static IResult EditTeamName(ApplicationContext db, TeamValidator teamValidator, ILogger logger, string teamName,
										string newTeamName, string username, string accessToken)
	{
		logger.LogInformation("Change team name: start");

		if (!teamValidator.Validate(new Services.Validation.Team(newTeamName))
				.IsValid)
		{
			logger.LogError("Change team name: wrong format of input");

			return Results.Json("Недопустимый формат ввода. Имя команды должно состоять только из букв и не превышть длину в 50 символов");
		}

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			newTeamName: newTeamName);

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