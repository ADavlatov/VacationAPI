using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Vacations.Put;

public class VacationEnd
{
	public static IResult EditVacationEnd(ApplicationContext db, string teamName, string employeeName, string vacationDateEnd,
											string newVacationDateEnd, string username,
											string accessToken)
	{
		if (db.Vacations.FirstOrDefault(x =>
				x.StartOfVacation == DateOnly.Parse(vacationDateEnd)
				&& x.Employee.Name == employeeName
				&& x.Employee.Team.Name == teamName
				&& x.Employee.Team.User.Name == username)
			!= null)
		{
			var vacation = db.Vacations.FirstOrDefault(x => x.StartOfVacation == DateOnly.Parse(vacationDateEnd));
			vacation.StartOfVacation = DateOnly.Parse(newVacationDateEnd);

			var response = new
			{
				oldVacationDateStart = vacationDateEnd,
				newVacationDateEnd
			};

			return Results.Json(response);
		}

		return Results.Json("");
	}
}