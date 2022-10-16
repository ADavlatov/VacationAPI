using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Get;

public class Employees
{
	public static IResult GetEmployees(ApplicationContext db, ILogger logger, string teamName, string username, string accessToken)
	{
		logger.LogInformation("Get employees: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName);
		Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username)!;

		if (request == null)
		{
			var teamEmployees = from employee in db.Employees
							where employee.Team == team
							select employee.Name;
			string employees = string.Join(", ", teamEmployees);

			logger.LogInformation("Get employees: successfully");

			return Results.Json(employees);
		}

		logger.LogError("Get employees: failed");

		return request;
	}
}