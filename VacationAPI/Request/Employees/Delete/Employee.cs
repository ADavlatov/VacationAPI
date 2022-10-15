using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Employees.Delete;

public class Employee
{
	public static IResult? RemoveEmployee(ApplicationContext db, string teamName, string employeeName, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName);
		Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName);

		if (employee != null && request == null)
		{
			db.Employees.Remove(employee);
			db.SaveChanges();

			return Results.Json($"Работник {employeeName} удален");
		}

		return request;
	}
}