using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Put;

public class VacationStart
{
	//Смена даты начала отпуска сотрудника
	public static IResult EditVacationStart(ApplicationContext db, ILogger logger, string teamName, string employeeName,
											string vacationDateStart,
											string vacationDateEnd,
											string newVacationDateStart, string username,
											string accessToken)
	{
		logger.LogInformation("Change vacation start date: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			vacationDateStart: vacationDateStart, newVacationDateStart: newVacationDateStart, vacationDateEnd: vacationDateEnd);

		if (request == null)
		{
			//Получение отпуска из базы данных
			Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
				x.StartOfVacation == DateOnly.Parse(vacationDateStart)
				&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
				&& x.Employee!.Name == employeeName
				&& x.Employee.Team.Name == teamName
				&& x.Employee.Team.User.Name == username)!;

			//Смена даты начала отпуска
			vacation.StartOfVacation = DateOnly.Parse(newVacationDateStart);
			db.SaveChanges();

			//Ответ в случае успеха, содержит, старую дату начала и окончания и новую дату начала отпуска
			var response = new
			{
				oldVacationDateStart = vacationDateStart,
				vacationDateEnd,
				newVacationDateStart
			};

			logger.LogInformation("Change vacation start date: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change vacation start date: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}