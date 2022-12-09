using VacationAPI.Contexts;
using VacationAPI.Entities;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Employees.Get;

public class DaysOnVacationAtTheMoment
{
	public static IResult GetInfoInGivenYear(ApplicationContext db, ILogger logger, string teamName, string employeeName, string year,
											string username,
											string accessToken)
	{
		logger.LogInformation("Get info about employee's days on vacation in a year count: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, year: year,
			teamName: teamName, employeeName: employeeName);

		if (result == null)
		{
			//получение списка отпусков сотрудника
			List<Vacation> vacations = (from vacation in db.Vacations
										where (vacation.StartOfVacation.Year == int.Parse(year)
												|| vacation.EndOfVacation.Year == int.Parse(year))
											&& vacation.Employee.Team.Name == teamName
											&& vacation.Employee.Name == employeeName
										select vacation).ToList();

			int daysOnVacation = 0;

			foreach (var vacation in vacations)
			{
				daysOnVacation += (int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0)))
					.TotalDays;

				if (vacation.StartOfVacation == vacation.EndOfVacation)
				{
					daysOnVacation += 1;
				}
			}

			//ответ пользователю в случае успеха, содержит имя сотрудника, название команды, год и количество дней в отпуске
			var response = new
			{
				employeeName,
				teamName,
				year,
				daysCount = daysOnVacation
			};

			logger.LogInformation("Get info about employee's days on vacation in a year count: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employee's days on vacation in a year count: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetInfoInGivenMonth(ApplicationContext db, ILogger logger, string teamName, string employeeName, string month,
											string year, string username,
											string accessToken)
	{
		logger.LogInformation("Get info about employee's days on vacation in a month count: start");

		//проверка валидности данных в запросе
		var result = Manager.CheckRequest(db, logger, username, accessToken, month: month,
			year: year,
			teamName: teamName, employeeName: employeeName);

		if (result == null)
		{
			//получение списка отпусков сотрудника
			List<Vacation> vacations = (from vacation in db.Vacations
										where ((vacation.StartOfVacation.Month == int.Parse(month)
												&& vacation.StartOfVacation.Year == int.Parse(year))
												|| (vacation.EndOfVacation.Month == int.Parse(month)
													&& vacation.EndOfVacation.Year == int.Parse(year)))
											&& vacation.Employee.Team.Name == teamName
											&& vacation.Employee.Name == employeeName
										select vacation).ToList();

			int daysOnVacation = 0;

			foreach (var vacation in vacations)
			{
				daysOnVacation += (int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0)))
					.TotalDays;

				if (vacation.StartOfVacation == vacation.EndOfVacation)
				{
					daysOnVacation += 1;
				}
			}

			//ответ пользователю в случае успеха, содержит имя сотрудника, название команды, год, месяц и количество дней в отпуске
			var response = new
			{
				employeeName,
				teamName,
				year,
				month,
				daysCount = daysOnVacation
			};

			logger.LogInformation("Get info about employee's days on vacation in a month count: successfully");

			return Results.Json(response);
		}

		logger.LogError("Get info about employee's days on vacation in a month count: failed");

		//ответ в случае ошибки в запросе, выявленной в классе Manager
		return result;
	}
}