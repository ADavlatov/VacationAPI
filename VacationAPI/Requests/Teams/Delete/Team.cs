using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Delete;

public class Team
{
	//Удаление команды пользоватея
	public static IResult RemoveTeam(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Delete team: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);

		if (request == null)
		{
			//Получение команды пользователя из базы данных
			Entities.Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username)!;

			//Удаление команды из базы данных
			db.Teams.Remove(team);
			db.SaveChanges();

			logger.LogInformation("Delete team: successfully");

			//Ответ в случае успеха
			return Results.Json($"Команда {teamName} удалена");
		}

		logger.LogError("Delete team: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}