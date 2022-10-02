using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Vacations.Put;

public class VacationStart
{
	public static IResult EditVacationStart(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
											string newVacationDateStart, string username,
											string accessToken)
	{
		if (db.Vacations.FirstOrDefault(x =>
				x.StartOfVacation == DateOnly.Parse(vacationDateStart)
				&& x.Employee.Name == employeeName
				&& x.Employee.Team.Name == teamName
				&& x.Employee.Team.User.Name == username)
			!= null)
		{
			var vacation = db.Vacations.FirstOrDefault(x => x.StartOfVacation == DateOnly.Parse(vacationDateStart));
			vacation.StartOfVacation = DateOnly.Parse(newVacationDateStart);

			var response = new
			{
				oldVacationDateStart = vacationDateStart,
				newVacationDateStart
			};

			return Results.Json(response);
		}

		return Results.Json("");
	}
}