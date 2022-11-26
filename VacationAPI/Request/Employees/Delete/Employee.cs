using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Employees.Delete;

public class Employee
{
	public static IResult RemoveEmployee(ApplicationContext db, ILogger logger, string teamName, string employeeName, string username,
										string accessToken)
	{
		logger.LogInformation("Delete employee: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName);

		if (request == null)
		{
			Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName)!;

			db.Employees.Remove(employee);
			db.SaveChanges();

			logger.LogInformation("Delete employee: successfully");

			return Results.Json($"Работник {employeeName} удален");
		}

		return request;
	}
}