using VacationAPI.Context;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Delete;

public class Vacation
{
	public static IResult RemoveVacation(ApplicationContext db, ILogger logger, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd,
										string username,
										string accessToken)
	{
		logger.LogInformation("Delete vacation: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee!.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username)!;

		if (request == null)
		{
			db.Vacations.Remove(vacation);
			db.SaveChanges();

			logger.LogInformation("Delete vacation: successfully");

			return Results.Json("Отпуск с началом в " + vacationDateStart + " удален");
		}

		logger.LogError("Delete vacation: failed");

		return request;
	}
}