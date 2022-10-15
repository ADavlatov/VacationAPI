using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Put;

public class EmployeeName
{
	public static IResult? EditEmployeeName(ApplicationContext db, string teamName, string oldEmployeeName, string newEmployeeName,
											string username,
											string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: oldEmployeeName,
			newEmployeeName: newEmployeeName);

		Entities.Employee employee =
			db.Employees.FirstOrDefault(x => x.Name == oldEmployeeName && x.Team.Name == teamName && x.Team.User.Name == username);

		if (employee != null && request == null)
		{
			employee.Name = newEmployeeName;
			db.SaveChanges();

			var response = new
			{
				teamName,
				oldEmployeeName,
				newEmployeeName
			};

			return Results.Json(response);
		}

		return request;
	}
}