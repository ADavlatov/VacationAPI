using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Employees.Put;

public class EmployeeName
{
	public static IResult EditEmployeeName(ApplicationContext db, string teamName, string oldEmployeeName, string newEmployeeName,
											string username,
											string accessToken)
	{
		var employee =
			db.Employees.FirstOrDefault(x => x.Name == oldEmployeeName && x.Team.Name == teamName && x.Team.User.Name == username);

		if (employee != null)
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

		return Results.Json("Работника с таким именем не существует");
	}
}