using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Get;

public class Employees
{
	public static IResult? GetEmployees(ApplicationContext db, string teamName, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName);
		Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username);

		if (team != null && request == null)
		{
			var teamEmployees = from employee in db.Employees
							where employee.Team == team
							select employee.Name;
			string employees = string.Join(", ", teamEmployees);

			return Results.Json(employees);
		}

		return request;
	}
}