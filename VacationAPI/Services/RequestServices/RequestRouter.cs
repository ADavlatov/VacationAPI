using VacationAPI.Contexts;
using VacationAPI.Services.Validation;

namespace VacationAPI.Services.RequestServices;

public class RequestRouter
{
	//@TODO добавить возможность перемещать сотрудников из одной команды в другую

	//Эндпоинты запросов к API
	public void Route(WebApplication app)
	{
		app.MapPost("/test/{username}/{accessToken}", (ApplicationContext db,
														ILogger<RequestRouter> logger, string username, string accessToken) =>
		{
			return Requests.User.Get.Diagrams.EmployeesOnVacation.GetDiagram(db, logger, username, accessToken);
		});

		//Регистрация пользователя. Добавление в бд
		app.MapPost("/api/user/sign-in/{username}/{password}", (ApplicationContext db, UserValidator userValidator,
																ILogger<RequestRouter> logger, string username, string password) =>
		{
			return Requests.User.Post.User.AddNewUser(db, userValidator, logger, username, password);
		});

		//Получение JWT токена
		app.MapGet("/api/user/log-in/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string password) =>
			{
				return Requests.User.Get.JwtToken.GetJwtToken(db, logger, username, password);
			});

		//Получение информации о командах пользователя
		app.MapGet("/api/user/teams/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string accessToken) =>
			{
				return Requests.User.Get.Teams.GetInfo(db, logger, username, accessToken);
			});

		//Получение информации о сотрудниках пользователя
		app.MapGet("/api/user/employees/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string accessToken) =>
			{
				return Requests.User.Get.Employees.GetInfo(db, logger, username, accessToken);
			});

		//Получение информации о сотрудниках в отпуске в данный момент
		app.MapGet("/api/user/employees/on-vacation/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string accessToken) =>
			{
				return Requests.User.Get.EmployeesOnVacation.GetInfo(db, logger, username, accessToken);
			});

		//Получение информации о сотрудниках в отпуске в заданный год
		app.MapGet("/api/user/employees/on-vacation/year/{year}/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string year, string username, string accessToken) =>
			{
				return Requests.User.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenYear(db, logger, year, username, accessToken);
			});

		//Получение информации о сотрудниках в отпуске в заданный месяц
		app.MapGet("/api/user/employees/on-vacation/month/{month}/year/{year}/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string month, string year, string username,
			string accessToken) =>
			{
				return Requests.User.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenMonth(db, logger, month, year, username,
					accessToken);
			});

		//Получение информации о сотрудниках в отпуске в заданный день
		app.MapGet("/api/user/employees/on-vacation/day/{day}/month/{month}/year/{year}/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string day, string month, string year,
			string username, string accessToken) =>
			{
				return Requests.User.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenDay(db, logger, day, month, year,
					username, accessToken);
			});

		//Получение диаграммы со всеми командами и колчеством сотрудников в отпуске в данный момент (количество сотрудников в каждой команде)
		app.MapGet("/api/user/diagrams/employees-on-vacation/now/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string accessToken) =>
			{
				return Requests.User.Get.Diagrams.EmployeesOnVacation.GetDiagram(db, logger, username, accessToken);
			});

		//Получение диаграммы со всеми командами и колчеством сотрудников в отпуске в заданном месяце (количество сотрудников на каждый день)
		app.MapGet("/api/user/diagrams/employees-on-vacation/per-month/{month}/year/{year}/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string month, string year, string username,
			string accessToken) =>
			{
				return Requests.User.Get.Diagrams.EmployeesOnVacationPerMonth.GetDiagram(db, logger, month, year, username,
					accessToken);
			});

		//Получение диаграммы со всеми командами и колчеством сотрудников в отпуске в заданном году (количество сотрудников на каждый месяц)
		app.MapGet("/api/user/diagrams/employees-on-vacation/per-year/{year}/{username}/{password}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string year, string username, string accessToken) =>
			{
				return Requests.User.Get.Diagrams.EmployeesOnVacationPerYear.GetDiagram(db, logger, year, username, accessToken);
			});

		//Создание новой команды
		app.MapPost("api/teams/new-team/{teamName}/{username}/{accessToken}",
			(ApplicationContext db, TeamValidator teamValidator, ILogger<RequestRouter> logger, string teamName, string username,
			string accessToken) =>
			{
				return Requests.Teams.Post.Team.AddNewTeam(db, teamValidator, logger, teamName, username,
					accessToken);
			});

		//Получение списка всех сотрудников команды
		app.MapGet("api/teams/{teamName}/employees/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string username, string accessToken) =>
			{
				return Requests.Teams.Get.Employees.GetInfo(db, logger, teamName, username, accessToken);
			});

		//Получение списка всех сотрудников команды в отпуске в данный момент
		app.MapGet("api/teams/{teamName}/employees/on-vacation/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string username, string accessToken) =>
			{
				return Requests.Teams.Get.EmployeesOnVacation.GetInfo(db, logger, teamName, username, accessToken);
			});

		//Получение списка всех сотрудников команды в отпуске в заданный год
		app.MapGet("api/teams/{teamName}/employees/on-vacation/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenYear(db, logger, teamName, year, username,
					accessToken);
			});

		//Получение списка всех сотрудников команды в отпуске в заданный месяц
		app.MapGet("api/teams/{teamName}/employees/on-vacation/month/{month}/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string month, string year,
			string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenMonth(db, logger, teamName, month, year,
					username,
					accessToken);
			});

		//Получение списка всех сотрудников команды в отпуске в заданный день
		app.MapGet("api/teams/{teamName}/employees/on-vacation/day/{day}/month/{month}/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string day, string month,
			string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.EmployeesOnVacationAtTheMoment.GetInfoInGivenDay(db, logger, teamName, day, month,
					year, username,
					accessToken);
			});

		// //Получение диаграммы с количеством отпусков всех команд за год по месяцам
		// app.MapGet("api/teams/stats-by-months/all-teams/vacations-count/per-year/{year}/{username}/{accessToken}",
		// 	(ApplicationContext db, ILogger<RequestRouter> logger, string year, string username, string accessToken) =>
		// 	{
		// 		return Request.Teams.Get.YearTeamsStats.GetTeamsStats(db, logger, year, username, accessToken);
		// 	});

		// //Получение диаграммы с количеством всех запланированных отпусков
		// app.MapGet("api/teams/stats/all-teams/vacations-count/planned/{username}/{accessToken}",
		// 	(ApplicationContext db, string username, string accessToken) =>
		// 	{
		// 		return VacationAPI.Request.Teams.Get.TeamsStats.PlannedVacations.GetTeamsStats(db, logger, username, accessToken);
		// 	});
		//
		// //Получение диаграммы с количеством всех сотрудников в отпуске в данный момент
		// app.MapGet("api/teams/stats/all-teams/employees-count/{username}/{accessToken}",
		// 	(ApplicationContext db, string username, string accessToken) =>
		// 	{
		// 		return VacationAPI.Request.Teams.Get.TeamsStats.Employees.GetTeamsStats(db, logger, username, accessToken);
		// 	});

		// //Получение диаграммы с количеством отпусков конкретной команды за год по месяцам
		// app.MapGet("api/teams/stats-by-months/team/{teamName}/vacations-count/current/per-year/{year}/{username}/{accessToken}",
		// 	(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string year, string username, string accessToken) =>
		// 	{
		// 		return Request.Teams.Get.YearTeamStats.GetTeamsStats(db, logger, teamName, year, username,
		// 			accessToken);
		// 	});

		// //Получение диаграммы с количеством всех запланированных отпусков
		// app.MapGet("api/teams/stats/team/vacations-count/planned/{teamName}/{username}/{accessToken}",
		// 	(ApplicationContext db, string teamName, string username, string accessToken) =>
		// 	{
		// 		return VacationAPI.Request.Teams.Get.TeamStats.PlannedVacations.GetTeamStats(db, logger, teamName, username, accessToken);
		// 	});

		//Получение диаграммы с сотрудниками команды в отпуске за заданный год по месяцам
		app.MapGet("api/teams/{teamName}/diagrams/employees-on-vacation/per-year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.Diagrams.EmployeesOnVacationPerYear.GetDiagram(db, logger, teamName, year, username,
					accessToken);
			});

		//Получение диаграммы с сотрудниками команды в отпуске за заданный месяц по дням
		app.MapGet("api/teams/{teamName}/diagrams/employees-on-vacation/per-month/{month}/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string month, string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.Diagrams.EmployeesOnVacationPerMonth.GetDiagram(db, logger, teamName, month, year, username,
					accessToken);
			});

		//Получение диаграммы с количеством отпусков сотрудников в днях за год
		app.MapGet("api/teams/{teamName}/diagrams/employee-vacations/per-year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.Diagrams.EmployeesOnVacationDaysCount.GetDiagramPerYear(db, logger, teamName, year, username,
					accessToken);
			});

		//Получение диаграммы с количеством отпусков сотрудников в днях за месяц
		app.MapGet("api/teams/{teamName}/diagrams/employee-vacations/per-month/{month}/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string month, string year, string username,
			string accessToken) =>
			{
				return Requests.Teams.Get.Diagrams.EmployeesOnVacationDaysCount.GetDiagramPerMonth(db, logger, teamName, month, year, username,
					accessToken);
			});

		//Изменение название команды
		app.MapPut("api/teams/team/{teamName}/{newTeamName}/{username}/{accessToken}",
			(ApplicationContext db, TeamValidator teamValidator, ILogger<RequestRouter> logger, string teamName, string newTeamName,
			string username,
			string accessToken) =>
			{
				return Requests.Teams.Put.TeamName.EditTeamName(db, teamValidator, logger, teamName, newTeamName,
					username,
					accessToken);
			});

		//Удаление команды
		app.MapDelete("api/teams/team/{teamName}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string username, string accessToken) =>
			{
				return Requests.Teams.Delete.Team.RemoveTeam(db, logger, teamName, username, accessToken);
			});

		//Добавление работника
		app.MapPost("api/teams/team/{teamName}/employees/new-employee/{employeeName}/{username}/{accessToken}",
			(ApplicationContext db, EmployeeValidator employeeValidator, ILogger<RequestRouter> logger, string teamName,
			string employeeName, string username,
			string accessToken) =>
			{
				return Requests.Employees.Post.Employee.AddNewEmployee(db, employeeValidator, logger, teamName, employeeName,
					username,
					accessToken);
			});

		// //Получение диаграммы с количеством отпусков конкретного работника за каждый месяц года
		// app.MapGet("api/teams/stats-by-months/team/{teamName}/employee/{employeeName}/per-year/{year}/{username}/{accessToken}",
		// 	(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string year, string username,
		// 	string accessToken) =>
		// 	{
		// 		return Request.Employees.Get.YearEmployeeStats.GetTeamsStats(db, logger, teamName, employeeName, year,
		// 			username, accessToken);
		// 	});

		//Получение количества дней в отпуске в заданном году работника заданной команды
		app.MapGet("api/teams/{teamName}/employees/{employeeName}/days-on-vacation/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string year,
			string username,
			string accessToken) =>
			{
				return Requests.Employees.Get.DaysOnVacationAtTheMoment.GetInfoInGivenYear(db, logger, teamName, employeeName, year,
					username, accessToken);
			});

		//Получение количества дней в отпуске в заданном месяце работника заданной команды
		app.MapGet("api/teams/{teamName}/employees/{employeeName}/days-on-vacation/month/{month}/year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string month,
			string year,
			string username,
			string accessToken) =>
			{
				return Requests.Employees.Get.DaysOnVacationAtTheMoment.GetInfoInGivenMonth(db, logger, teamName, employeeName, month,
					year,
					username, accessToken);
			});

		//Изменение имени работника
		app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeeName}/{username}/{accessToken}",
			(ApplicationContext db, EmployeeValidator employeeValidator, ILogger<RequestRouter> logger, string teamName,
			string employeeName, string newEmployeeName,
			string username,
			string accessToken) =>
			{
				return Requests.Employees.Put.EmployeeName.EditEmployeeName(db, employeeValidator, logger, teamName, employeeName,
					newEmployeeName,
					username,
					accessToken);
			});

		//Удаление работника
		app.MapDelete("api/teams/{teamName}/employees/employee/{employeeName}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string username,
			string accessToken) =>
			{
				return Requests.Employees.Delete.Employee.RemoveEmployee(db, logger, teamName, employeeName, username,
					accessToken);
			});

		//Добавление отпуска сотруднику
		app.MapPost(
			"api/teams/{teamName}/employees/{employeeName}/vacations/new-vacation/{vacationDateStart}/{vacationDateEnd}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string vacationDateStart,
			string vacationDateEnd,
			string username,
			string accessToken) =>
			{
				return Request.Vacations.Post.Vacation.AddNewVacation(db, logger, teamName, employeeName, vacationDateStart,
					vacationDateEnd,
					username, accessToken);
			});

		//@TODO сделать вывод даты начала и даты окончания каждого отпуска
		// //список отпусков работника
		// app.MapGet("api/teams/{teamName}/employees/{employeeName}/vacations-count/{username}/{accessToken}", (
		// 	ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string username,
		// 	string accessToken) =>
		// {
		// 	return Request.Vacations.Get.Employee.GetVacations(db, logger, teamName, employeeName, username,
		// 		accessToken);
		// });

		//Изменение начала отпуска сотрудника
		app.MapPut(
			"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/vacationDateStart/{newDate}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string vacationDateStart,
			string vacationDateEnd,
			string newDate,
			string username,
			string accessToken) =>
			{
				return Request.Vacations.Put.VacationStart.EditVacationStart(db, logger, teamName, employeeName,
					vacationDateStart,
					vacationDateEnd,
					newDate, username, accessToken);
			});

		//Изменение конца отпуска сотрудника
		app.MapPut(
			"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/vacationDateEnd/{newDate}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string vacationDateStart,
			string vacationDateEnd,
			string newDate, string username,
			string accessToken) =>
			{
				return Request.Vacations.Put.VacationEnd.EditVacationEnd(db, logger, teamName, employeeName, vacationDateStart,
					vacationDateEnd,
					newDate, username, accessToken);
			});

		//Удаление отпуска сотрудника
		app.MapDelete(
			"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string vacationDateStart,
			string vacationDateEnd,
			string username,
			string accessToken) =>
			{
				return Request.Vacations.Delete.Vacation.RemoveVacation(db, logger, teamName, employeeName, vacationDateStart,
					vacationDateEnd,
					username,
					accessToken);
			});
	}
}