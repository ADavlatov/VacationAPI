using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get;

public class EmployeesOnVacation
{
	public static IResult GetInfo(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's employees on vacation: start");

		//Проверка валдиности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			List<string> employees = new List<string>();

			//плдучение списка сотрудников в отпуске в данный момент времени
			foreach (var vacation in db.Vacations)
			{
				if (vacation.StartOfVacation < DateOnly.FromDateTime(DateTime.Now.Date)
					&& vacation.EndOfVacation > DateOnly.FromDateTime(DateTime.Now.Date))
				{
					employees.Add(vacation.Employee!.Name);
				}
			}

			//ответ в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeeCount = employees.Count,
				employeeList= string.Join(", ", employees)
			};

			logger.LogInformation("Get info about user's employees on vacation: successfully");

			//ответ в случае успеха
			return Results.Json(response);
		}

		logger.LogError("Get info about user's employees on vacation: failed");

		//ответ в случае ошибки в заросе, выявленной в классе Manager
		return result;
	}
}