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

		if (JwtToken.CheckJwtToken(username, accessToken)
			&& employee != null)
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

		if (!JwtToken.CheckJwtToken(username, accessToken))
		{
			Results.Json("Неверный access_token");
		}

		return Results.Json("Работника с таким именем не существует");
	}
}