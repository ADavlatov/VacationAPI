using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Post;

public class Vacation
{
	public static IResult AddNewVacation(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string vacationDateStart,
										string vacationDateEnd, string username, string accessToken)
	{
		logger.LogInformation("Add new vacation: start");

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			newVacationDateStart: vacationDateStart, newVacationDateEnd: vacationDateEnd);

		Entities.Employee employee =
			db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

		if (request == null)
		{
			db.Vacations.Add(new()
			{
				StartOfVacation = DateOnly.Parse(vacationDateStart),
				EndOfVacation = DateOnly.Parse(vacationDateEnd),
				Employee = employee
			});

			db.SaveChanges();

			logger.LogInformation("Add new vacation: successfully");

			return Results.Json($"Отпуск с началом {vacationDateStart} и концом {vacationDateEnd} создан");
		}

		logger.LogError("Add new vacation: failed");

		return request;
	}
}