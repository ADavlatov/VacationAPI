using VacationAPI.Context;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Put;

public class VacationStart
{
	public static IResult? EditVacationStart(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
											string vacationDateEnd,
											string newVacationDateStart, string username,
											string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd, newVacationDateStart: newVacationDateStart);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username);

		if (vacation != null && request == null)
		{
			vacation.StartOfVacation = DateOnly.Parse(newVacationDateStart);
			db.SaveChanges();

			var response = new
			{
				oldVacationDateStart = vacationDateStart,
				vacationDateEnd,
				newVacationDateStart
			};

			return Results.Json(response);
		}

		return request;
	}
}