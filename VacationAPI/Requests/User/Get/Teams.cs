using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get;

public class Teams
{
	public static IResult GetInfo(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's teams: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			//получение списка команд пользователя
			List<string> teams = (from team in db.Teams
								where team.User.Name == username
								select team.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				teamsCount = teams.Count,
				teamsName = string.Join(", ", teams)
			};

			logger.LogInformation("Get info about user's teams: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about user's teams: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}