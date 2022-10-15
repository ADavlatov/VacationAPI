using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.RequestManager;

namespace VacationAPI.Request.Vacations.Post;

public class Vacation
{
	public static IResult? AddNewVacation(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
										string vacationDateEnd, string username, string accessToken)
	{
		var request = Manager.CheckRequest(db, username, accessToken, teamName: teamName, employeeName: employeeName,
			newVacationDateStart: vacationDateStart, newVacationDateEnd: vacationDateEnd);

		Entities.Employee employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.User.Name == username);

		if (employee != null && request == null)
		{
			Console.WriteLine(db.Vacations.Count());
			db.Vacations.Add(new()
			{
				StartOfVacation = DateOnly.Parse(vacationDateStart),
				EndOfVacation = DateOnly.Parse(vacationDateEnd),
				Employee = employee
			});

			db.SaveChanges();

			return Results.Json($"Отпуск с началом {vacationDateStart} и концом {vacationDateEnd} создан");
		}

		return request;
	}
}