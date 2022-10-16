using VacationAPI.Context;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Put;

public class EmployeeName
{
	public static IResult EditEmployeeName(ApplicationContext db, ILogger logger, string teamName, string oldEmployeeName, string newEmployeeName,
											string username,
											string accessToken)
	{
		logger.LogInformation("Change employee name: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName, employeeName: oldEmployeeName,
			newEmployeeName: newEmployeeName);

		Entities.Employee employee =
			db.Employees.FirstOrDefault(x => x.Name == oldEmployeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

		if (request == null)
		{
			employee.Name = newEmployeeName;
			db.SaveChanges();

			var response = new
			{
				teamName,
				oldEmployeeName,
				newEmployeeName
			};

			logger.LogInformation("Change employee name: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change employee name: failed");

		return request;
	}
}