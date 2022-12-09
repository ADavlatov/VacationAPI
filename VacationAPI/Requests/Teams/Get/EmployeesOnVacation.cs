using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Get;

public class EmployeesOnVacation
{
	public static IResult GetInfo(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Get info about employees on vacation in team: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);

		if (result == null)
		{
			List<string> employees = new List<string>();

			//получение списка сотрудников команды
			foreach (var vacation in db.Vacations)
			{
				if (vacation.StartOfVacation < DateOnly.FromDateTime(DateTime.Now)
					&& vacation.EndOfVacation > DateOnly.FromDateTime(DateTime.Now)
					&& vacation.Employee!.Team.Name == teamName)
				{
					employees.Add(vacation.Employee!.Name);
				}
			}

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeeCount = employees.Count,
				employeeList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about employees on vacation in team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employees on vacation in team: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}