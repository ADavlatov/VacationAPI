using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using Team = VacationAPI.Entities.Team;

namespace VacationAPI.Requests.Teams.Put;

public class TeamName
{
	//Изменение имени команды
	public static IResult EditTeamName(ApplicationContext db, TeamValidator teamValidator, ILogger logger, string teamName,
										string newTeamName, string username, string accessToken)
	{
		logger.LogInformation("Change team name: start");

		//Проверка имени команды на валидность
		if (!teamValidator.Validate(new Services.Validation.Team(newTeamName))
				.IsValid)
		{
			logger.LogError("Change team name: wrong format of input");

			//Ответ в случае ошибки в имени команды
			return Results.Json("Недопустимый формат ввода. Имя команды должно состоять только из букв и не превышть длину в 50 символов");
		}

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			newTeamName: newTeamName);

		if (request == null)
		{
			//Получение заданной команды
			Team team = db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName && x.Name != newTeamName)!;

			//Изменение имени команды
			team.Name = newTeamName;
			db.SaveChanges();

			//Ответ в случае успеха, содержит старое и новое имя
			var response = new
			{
				oldTeamName = teamName,
				newTeamName
			};

			logger.LogInformation("Change team name: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change team name: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}