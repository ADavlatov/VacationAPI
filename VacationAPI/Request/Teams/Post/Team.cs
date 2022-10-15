using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Teams.Post;

public class Team
{
	public static IResult AddNewTeam(ApplicationContext db, string teamName, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, newTeamName: teamName);
		User user = db.Users.FirstOrDefault(x => x.Name == username);

		if (user != null && request == null)
		{
			db.Teams.Add(new()
			{
				User = user,
				Name = teamName
			});

			db.SaveChanges();

			var response = new
			{
				username,
				teamName
			};

			return Results.Json(response);
		}

		return request;
	}
}