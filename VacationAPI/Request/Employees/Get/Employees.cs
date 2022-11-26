using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Employees.Get;

public class Employees
{
	//Получение списка имен сотрудников заданной команды через запятую
	public static IResult GetEmployees(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Get employees: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);

		if (request == null)
		{
			//Получение нужной команды из базы данных
			Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username)!;

			//Получение сотрудников команды
			var teamEmployees = from employee in db.Employees
								where employee.Team == team
								select employee.Name;

			//Получение строки с перечислением сотрудников через запятую
			string employees = string.Join(", ", teamEmployees);

			logger.LogInformation("Get employees: successfully");

			//Ответ пользователю в случае успеха
			return Results.Json(employees);
		}

		logger.LogError("Get employees: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manger
		return request;
	}
}