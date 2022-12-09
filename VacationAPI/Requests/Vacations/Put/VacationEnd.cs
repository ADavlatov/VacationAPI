using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Put;

public class VacationEnd
{
	//Cмена даты окончания отпуска сотрудника
	public static IResult EditVacationEnd(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string vacationDateStart,
										string vacationDateEnd,
										string newVacationDateEnd, string username,
										string accessToken)
	{
		logger.LogInformation("Change vacation end date: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd, newVacationDateEnd: newVacationDateEnd);

		if (request == null)
		{
			//Получение отпуска из базы данных
			Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
				x.StartOfVacation == DateOnly.Parse(vacationDateStart)
				&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
				&& x.Employee!.Name == employeeName
				&& x.Employee.Team.Name == teamName
				&& x.Employee.Team.User.Name == username)!;

			//Смена даты окончания отпуска
			vacation.EndOfVacation = DateOnly.Parse(newVacationDateEnd);
			db.SaveChanges();

			//Ответ в случае успеха, содержит дату начала, старую и новую дату окончания отпуска
			var response = new
			{
				vacationDateStart,
				oldVacationDateEnd = vacationDateEnd,
				newVacationDateEnd
			};

			logger.LogInformation("Change vacation end date: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change vacation end date: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}