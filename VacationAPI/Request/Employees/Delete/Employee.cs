using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Delete;

public class Employee
{
	public static IResult? RemoveEmployee(ApplicationContext db, ILogger logger, string teamName, string employeeName, string username,
										string accessToken)
	{
		logger.LogInformation("Delete employee: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName);

		Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName);

		if (employee != null && request == null)
		{
			db.Employees.Remove(employee);
			db.SaveChanges();

			logger.LogInformation("Delete employee: successfully");

			return Results.Json($"Работник {employeeName} удален");
		}

		logger.LogError("Delete employee: failed");

		return request;
	}
}