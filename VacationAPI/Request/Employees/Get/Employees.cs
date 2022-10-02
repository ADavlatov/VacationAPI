using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Employees.Get;

public class Employees
{
	public static IResult GetEmployees(ApplicationContext db, string teamName, string username, string accessToken)
	{
		if (db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username) != null)
		{
			Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username);
			string employees = string.Join(", ", team.Employees);

			return Results.Json(employees);
		}

		return Results.Json("Команды с таким именем не существует");
	}
}