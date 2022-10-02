using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;

namespace VacationAPI.Request.Vacations.Post;

public class Vacation
{
	public static IResult AddNewVacation(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd, string username, string accessToken)
	{
		DateOnly vacationStart;
		DateOnly vacationEnd;

		if (DateOnly.TryParse(vacationDateStart, out vacationStart) && DateOnly.TryParse(vacationDateEnd, out vacationEnd))
		{
			if (JwtToken.CheckJwtToken(username, accessToken)
				&& db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username)
				!= null)
			{
				db.Vacations.Add(new()
				{
					StartOfVacation = vacationStart,
					EndOfVacation = vacationEnd,
					Employee = db.Employees.FirstOrDefault(x => x.Name == employeeName)
				});
			} else if (!JwtToken.CheckJwtToken(username, accessToken))
			{
				Results.Json("Неверный access_token");
			} else
			{
				return Results.Json("Пользователь или команда с таким именем не существуют");
			}
		}

		return Results.Json("Неправильный формат даты");
	}
}