using VacationAPI.Context;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Request.Vacations.Post;

public class Vacation
{
	//Добавление нового отпуска сотрудника
	public static IResult AddNewVacation(ApplicationContext db, ILogger logger, string teamName, string employeeName,
										string vacationDateStart,
										string vacationDateEnd, string username, string accessToken)
	{
		logger.LogInformation("Add new vacation: start");

		//Проверка вадидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: employeeName,
			newVacationDateStart: vacationDateStart, newVacationDateEnd: vacationDateEnd);

		if (request == null)
		{
			//Получение сотрудника из базы данных
			Entities.Employee employee =
				db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

			//Добавление нового отпуска в базу данных
			db.Vacations.Add(new()
			{
				StartOfVacation = DateOnly.Parse(vacationDateStart), //начало отпуска
				EndOfVacation = DateOnly.Parse(vacationDateEnd), //Конец отпуска
				Employee = employee //Сотрудник
			});

			db.SaveChanges();

			logger.LogInformation("Add new vacation: successfully");

			//Ответ в случае успеха
			return Results.Json($"Отпуск с началом {vacationDateStart} и концом {vacationDateEnd} создан");
		}

		logger.LogError("Add new vacation: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}