using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Employees.Put;

public class EmployeePosition
{
	public static IResult EditEmployeePosition(ApplicationContext db, string teamName, string employeeName, string newEmployeePosition,
												string username,
												string accessToken)
	{
		var employee =
			db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username);

		if (employee != null)
		{
			employee.Position = newEmployeePosition;
			db.SaveChanges();

			var response = new
			{
				teamName,
				employeeName,
				newEmployeePosition
			};

			return Results.Json(response);
		}

		return Results.Json("Работника с таким именем не существует");
	}
}