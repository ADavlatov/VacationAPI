using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using User = VacationAPI.Entities.User;

namespace VacationAPI.Request.Teams.Post;

public class Team
{
	//Добавление новой команды пользователя
	public static IResult AddNewTeam(ApplicationContext db, TeamValidator teamValidator, ILogger logger, string teamName, string username,
									string accessToken)
	{
		logger.LogInformation("Add new team: start");

		//Проверка имени команды на валидность
		if (!teamValidator.Validate(new Services.Validation.Team(teamName))
				.IsValid)
		{
			logger.LogError("Add new team: wrong format of input");

			//Ответ в случае ошибки в имени команды
			return Results.Json("Недопустимый формат ввода. Имя команды должно состоять только из букв и не превышть длину в 50 символов");
		}

		//Проверка валдиности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, newTeamName: teamName);

		if (request == null)
		{
			//Получение пользователя
			User user = db.Users.FirstOrDefault(x => x.Name == username)!;

			//Добавление команды в базу данных
			db.Teams.Add(new()
			{
				User = user,
				Name = teamName
			});

			db.SaveChanges();

			//Ответ в случае успеха, содержит имя ползователя и имя команды
			var response = new
			{
				username,
				teamName
			};

			logger.LogInformation("Add new team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Add new team: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}