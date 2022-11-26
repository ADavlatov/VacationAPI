using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Put;

public class VacationStart
{
	public static IResult EditVacationStart(ApplicationContext db, ILogger logger, string teamName, string employeeName,
											string vacationDateStart,
											string vacationDateEnd,
											string newVacationDateStart, string username,
											string accessToken)
	{
		logger.LogInformation("Change vacation start date: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			vacationDateStart: vacationDateStart, newVacationDateStart: newVacationDateStart, vacationDateEnd: vacationDateEnd);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee!.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username)!;

		if (request == null)
		{
			vacation.StartOfVacation = DateOnly.Parse(newVacationDateStart);
			db.SaveChanges();

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

		return request;
	}
}