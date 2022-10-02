using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Employees.Delete;

public class Employee
{
	public static IResult RemoveEmployee(ApplicationContext db, string teamName, string employeeName, string username, string accessToken)
	{
		var employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username);

		if (employee != null)
		{
			db.Employees.Remove(employee);
			db.SaveChanges();

			return Results.Json($"Работник {employeeName} удален");
		}

		return Results.Json("Работника или команды с таким именем не существует");
	}
}