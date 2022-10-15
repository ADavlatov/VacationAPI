using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Put;

public class VacationEnd
{
	public static IResult EditVacationEnd(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd,
										string newVacationDateEnd, string username,
										string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd, newVacationDateEnd: newVacationDateEnd);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username);

		if (vacation != null && request == null)
		{
			vacation.EndOfVacation = DateOnly.Parse(newVacationDateEnd);
			db.SaveChanges();

			var response = new
			{
				vacationDateStart,
				oldVacationDateEnd = vacationDateEnd,
				newVacationDateEnd
			};

			return Results.Json(response);
		}

		return request;
	}
}