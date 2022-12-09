using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Delete;

public class Vacation
{
	//Удаление отпуска сотрудника
	public static IResult RemoveVacation(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string vacationDateStart,
										string vacationDateEnd,
										string username,
										string accessToken)
	{
		logger.LogInformation("Delete vacation: start");

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd);

		if (request == null)
		{
			//Получение отпуска сотрудника из базы данных
			Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
				x.StartOfVacation == DateOnly.Parse(vacationDateStart)
				&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
				&& x.Employee!.Name == employeeName
				&& x.Employee.Team.Name == teamName
				&& x.Employee.Team.User.Name == username)!;

			//Удаление отпуска сотрудника из базы данных
			db.Vacations.Remove(vacation);
			db.SaveChanges();

			logger.LogInformation("Delete vacation: successfully");

			//Ответ в случае успеха
			return Results.Json("Отпуск с началом в " + vacationDateStart + " удален");
		}

		logger.LogError("Delete vacation: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}