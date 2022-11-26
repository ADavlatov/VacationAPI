using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using User = VacationAPI.Entities.User;

namespace VacationAPI.Request.Teams.Post;

public class Team
{
	public static IResult AddNewTeam(ApplicationContext db, TeamValidator teamValidator, ILogger logger, string teamName, string username,
									string accessToken)
	{
		logger.LogInformation("Add new team: start");

		if (!teamValidator.Validate(new Services.Validation.Team(teamName))
				.IsValid)
		{
			logger.LogError("Add new team: wrong format of input");

			return Results.Json("Недопустимый формат ввода. Имя команды должно состоять только из букв и не превышть длину в 50 символов");
		}

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