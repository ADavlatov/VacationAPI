using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get;

public class EmployeesOnVacationAtTheMoment
{
	public static IResult GetInfoInGivenYear(ApplicationContext db, ILogger logger, string year, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's employees on vacation in a given year: start");

		//Проверка валдиности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, year: year);

		if (result == null)
		{
			//получение списка сотрудников в отпуске в заданном году
			List<string> employees = (from vacation in db.Vacations
										where vacation.StartOfVacation.Year == int.Parse(year)
											|| vacation.EndOfVacation.Year == int.Parse(year)
										select vacation.Employee.Name).ToList();

			//ответ в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about user's employees on vacation in a given year: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about user's employees on vacation in a given year: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetInfoInGivenMonth(ApplicationContext db, ILogger logger, string month, string year, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's employees on vacation in a given month: start");

		var result = Manager.CheckRequest(db, logger, username, accessToken, month: month, year: year);

		if (result == null)
		{
			//получение списка сотрудников в отпуске в заданном месяце
			List<string> employees = (from vacation in db.Vacations
									where vacation.StartOfVacation.Month == int.Parse(month)
										|| vacation.EndOfVacation.Month == int.Parse(month)
									select vacation.Employee.Name).ToList();

			//ответ в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about user's employees on vacation in a given month: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about user's employees on vacation in a given month: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetInfoInGivenDay(ApplicationContext db, ILogger logger, string day, string month, string year, string username, string accessToken)
	{
		logger.LogInformation("Get info about user's employees on vacation in a given day: start");

		var result = Manager.CheckRequest(db, logger, username, accessToken, day: day, month: month, year: year);

		if (result == null)
		{
			//получение списка сотрудников в отпуске в заданном дне
			List<string> employees = (from vacation in db.Vacations
									where vacation.StartOfVacation.Day == int.Parse(day)
										|| vacation.EndOfVacation.Day == int.Parse(day)
									select vacation.Employee.Name).ToList();

			//ответ в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about user's employees on vacation in a given day: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about user's employees on vacation in a given day: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}