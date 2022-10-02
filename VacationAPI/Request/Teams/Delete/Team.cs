using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Teams.Delete;

public class Team
{
	public static IResult RemoveTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		if (JwtToken.CheckJwtToken(username, accessToken) && db.Teams.FirstOrDefault(x => x.Name == teamName) != null)
		{
			db.Teams.Remove(db.Teams.FirstOrDefault(x => x.Name == teamName));
			db.SaveChanges();

			return Results.Json($"Команда {teamName} удалена");
		}

		if (!JwtToken.CheckJwtToken(username, accessToken))
		{
			Results.Json("Неверный access_token");
		}

		return Results.Json("Команды с таким именем не существует");
	}
}