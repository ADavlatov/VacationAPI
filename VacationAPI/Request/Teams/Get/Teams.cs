using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Teams.Get;

public class Teams
{
	//Получение списка имен команд через запятую
	public static IResult GetTeams(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		//Проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			//Получение пользователя
			Entities.User user = db.Users.FirstOrDefault(x => x.Name == username)!;

			//Получение имен команд пользователя
			var teamsName = from team in db.Teams
							where team.User == user
							select team.Name;

			//Получение строки с перечислением всех команд пользователя через запятую
			string teams = string.Join(", ", teamsName);

			return Results.Json(teams);
		}

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}