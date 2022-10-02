using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Employees.Post;

public class Employee
{
	public static IResult AddNewEmployee(ApplicationContext db, string teamName, string employeeName, string employeePosition,
										string username,
										string accessToken)
	{
		if (db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username) != null)
		{
			db.Employees.Add(new()
			{
				Name = employeeName,
				Position = employeePosition,
				Team = db.Teams.FirstOrDefault(x => x.User.Name == username)
			});

			db.SaveChanges();

			var response = new
			{
				teamName,
				employeeName,
				employeePosition
			};

			return Results.Json(response);
		}

		return Results.Json("Команды с таким именем не существует");
	}
}