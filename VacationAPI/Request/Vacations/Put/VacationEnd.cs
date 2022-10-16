using VacationAPI.Context;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Put;

public class VacationEnd
{
	public static IResult EditVacationEnd(ApplicationContext db, ILogger logger, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd,
										string newVacationDateEnd, string username,
										string accessToken)
	{
		logger.LogInformation("Change vacation end date: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd, newVacationDateEnd: newVacationDateEnd);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee!.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username)!;

		if (request == null)
		{
			vacation.EndOfVacation = DateOnly.Parse(newVacationDateEnd);
			db.SaveChanges();

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

		return request;
	}
}