using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Post;

public class Employee
{
	public static IResult? AddNewEmployee(ApplicationContext db, string teamName, string employeeName,
										string username,
										string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, newEmployeeName: employeeName);
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

			return Results.Json(response);
		}

		return request;
	}
}