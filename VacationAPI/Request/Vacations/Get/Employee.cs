using VacationAPI.Context;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Get;

public class Employee
{
	public static IResult GetVacations(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string username, string accessToken)
	{
		logger.LogInformation("Get vacations: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, employeeName: employeeName);

		Entities.Employee employee =
			db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

		if (request == null)
		{
			var employeeVacations = from vacation in db.Vacations
									where vacation.Employee == employee
									select vacation;

			string teams = string.Join(", ", employeeVacations.Count());

			logger.LogInformation("Get vacations: successfully");

			return Results.Json(teams);
		}

		logger.LogError("Get vacations: failed");

		return request;
	}
}