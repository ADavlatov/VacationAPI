using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Post;

public class Employee
{
	public static IResult? AddNewEmployee(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string username,
										string accessToken)
	{
		logger.LogInformation("Add new employee: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, newEmployeeName: employeeName);
		Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username);

		if (team != null && request == null)
		{
			db.Employees.Add(new()
			{
				Name = employeeName,
				Team = team
			});

			db.SaveChanges();

			var response = new
			{
				teamName,
				employeeName,
			};

			logger.LogInformation("Add new employee: successfully");

			return Results.Json(response);
		}

		logger.LogError("Add new employee: failed");

		return request;
	}
}