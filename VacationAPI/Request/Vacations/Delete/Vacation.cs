using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Vacations.Delete;

public class Vacation
{
	public static IResult RemoveVacation(ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
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
			db.Vacations.Remove(vacation);
			db.SaveChanges();

			return Results.Json("Отпуск с началом в " + vacationDateStart + " удален");
		}

		return Results.Json("");
	}
}