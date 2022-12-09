using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get;

public class Employees
{
	public static IResult GetInfo(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's employees: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			//получение списка сотрудников пользователя
			List<string> employees = (from employee in db.Employees
												where employee.Team.User.Name == username
												select employee.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about user's employees: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about user's employees: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}