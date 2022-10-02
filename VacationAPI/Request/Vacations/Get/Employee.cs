using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Vacations.Get;

public class Employee
{
	public static IResult GetVacations(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
										string username, string accessToken)
	{
		if (db.Employees.FirstOrDefault(x =>
				x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username && x.Vacations.Any())
			!= null)
		{
			Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName);
			string teams = string.Join(", ", employee.Vacations);

			return Results.Json(teams);
		}

		return Results.Json("Список отпусков пуст");
	}
}