using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Teams.Delete;

public class Team
{
	public static IResult RemoveTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		if (db.Teams.FirstOrDefault(x => x.Name == teamName) != null)
		{
			db.Teams.Remove(db.Teams.FirstOrDefault(x => x.Name == teamName));
			db.SaveChanges();

			return Results.Json($"Команда {teamName} удалена");
		}

		return Results.Json("Команды с таким именем не существует");
	}
}