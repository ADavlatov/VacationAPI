using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Get;

public class Employee
{
	public static IResult? GetVacations(ApplicationContext db, string teamName, string employeeName,
										string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName);

		Entities.Employee employee =
			db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username);

		if (employee != null && request == null)
		{
			var employeeVacations = from vacation in db.Vacations
									where vacation.Employee == employee
									select vacation;
			string teams = string.Join(", ", employeeVacations.Count());

			return Results.Json(teams);
		}

		return request;
	}
}