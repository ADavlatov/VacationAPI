using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Delete;

public class Vacation
{
	public static IResult RemoveVacation(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd,
										string username,
										string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName,
			vacationDateStart: vacationDateStart, vacationDateEnd: vacationDateEnd);

		Entities.Vacation vacation = db.Vacations.FirstOrDefault(x =>
			x.StartOfVacation == DateOnly.Parse(vacationDateStart)
			&& x.EndOfVacation == DateOnly.Parse(vacationDateEnd)
			&& x.Employee.Name == employeeName
			&& x.Employee.Team.Name == teamName
			&& x.Employee.Team.User.Name == username);

		if (vacation != null && request == null)
		{
			db.Vacations.Remove(vacation);
			db.SaveChanges();

			return Results.Json("Отпуск с началом в " + vacationDateStart + " удален");
		}

		return request;
	}
}