using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Get;

public class Employees
{
	public static IResult GetInfo(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Get info about employees in team: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);

		if (result == null)
		{
			//получение списка сотрудников команды
			List<string> employees = (from employee in db.Employees
									where employee.Team.User.Name == username && employee.Team.Name == teamName
									select employee.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about employees in team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employees in team: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}