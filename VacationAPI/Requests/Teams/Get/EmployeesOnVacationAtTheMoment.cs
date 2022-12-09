using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Get;

public class EmployeesOnVacationAtTheMoment
{
	public static IResult GetInfoInGivenYear(ApplicationContext db, ILogger logger, string teamName, string year, string username,
											string accessToken)
	{
		logger.LogInformation("Get info about employees on vacation in a given year in team: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, year: year,
			teamName: teamName);

		if (result == null)
		{
			//получение списка сотрудников команды
			List<string> employees = (from vacation in db.Vacations
									where (vacation.StartOfVacation.Year == int.Parse(year)
											|| vacation.EndOfVacation.Year == int.Parse(year))
										&& vacation.Employee.Team.Name == teamName
									select vacation.Employee.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about employees on vacation in a given year in team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employees on vacation in a given year in team: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetInfoInGivenMonth(ApplicationContext db, ILogger logger, string teamName, string month, string year,
											string username,
											string accessToken)
	{
		logger.LogInformation("Get info about employees on vacation in a given month in team: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, month: month,
			year: year,
			teamName: teamName);

		if (result == null)
		{
			//получение списка сотрудников команды
			List<string> employees = (from vacation in db.Vacations
									where ((vacation.StartOfVacation.Month == int.Parse(month)
											&& vacation.StartOfVacation.Year == int.Parse(year))
											|| (vacation.EndOfVacation.Month == int.Parse(month)
												&& vacation.EndOfVacation.Year == int.Parse(year)))
										&& vacation.Employee.Team.Name == teamName
									select vacation.Employee.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about employees on vacation in a given month in team: successfully");

			//ответ в случае ошибки в запросе, выявленной в классе Manager
			return Results.Json(response);
		}

		logger.LogError("Get info about employees on vacation in a given month in team: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetInfoInGivenDay(ApplicationContext db, ILogger logger, string teamName, string day, string month,
											string year, string username,
											string accessToken)
	{
		logger.LogInformation("Get info about employees on vacation in a given day in team: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, day: day,
			month: month, year: year,
			teamName: teamName);

		if (result == null)
		{
			//получение списка сотрудников команды
			List<string> employees = (from vacation in db.Vacations
									where ((vacation.StartOfVacation.Day == int.Parse(day)
											&& vacation.StartOfVacation.Month == int.Parse(month)
											&& vacation.StartOfVacation.Year == int.Parse(year))
											|| (vacation.EndOfVacation.Day == int.Parse(day)
												&& vacation.EndOfVacation.Month == int.Parse(month)
												&& vacation.EndOfVacation.Year == int.Parse(year)))
										&& vacation.Employee.Team.Name == teamName
									select vacation.Employee.Name).ToList();

			//ответ пользователю в случае успеха, содержит количество сотрудников и их список через запятую
			var response = new
			{
				employeesCount = employees.Count,
				employeesList = string.Join(", ", employees)
			};

			logger.LogInformation("Get info about employees on vacation in a given day in team: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employees on vacation in a given day in team: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}