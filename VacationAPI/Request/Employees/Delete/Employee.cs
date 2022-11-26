using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Employees.Delete;

public class Employee
{
	//Удаление сотрудника заданной команды
	public static IResult RemoveEmployee(ApplicationContext db, ILogger logger, string teamName, string employeeName, string username,
										string accessToken)
	{
		logger.LogInformation("Delete employee: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName);

		if (request == null)
		{
			//Получение нужного сотрудника из базы данных
			Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

			//Удаление сотрудника из базы данных
			db.Employees.Remove(employee);
			db.SaveChanges();

			logger.LogInformation("Delete employee: successfully");

			//Ответ в случае успеха
			return Results.Json($"Работник {employeeName} удален");
		}

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}