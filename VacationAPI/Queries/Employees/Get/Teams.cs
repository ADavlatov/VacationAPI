using VacationAPI.Authentication;
using VacationAPI.Context;
using VacationAPI.Entities;

namespace VacationAPI.Queries.Employees.Get;

public class Teams
{
	public static IResult GetTeams(ApplicationContext db, string username, string accessToken)
	{
		if (JwtToken.CheckJwtToken(username, accessToken) && db.Users.FirstOrDefault(x => x.Name == username && x.Teams.Any()) != null)
		{
			User user = db.Users.FirstOrDefault(x => x.Name == username);
			string teams = string.Join(", ", user.Teams);

			return Results.Json(teams);
		}

		if (!JwtToken.CheckJwtToken(username, accessToken))
		{
			Results.Json("Неверный access_token");
		}

		return Results.Json("Список команд пуст");
	}
}