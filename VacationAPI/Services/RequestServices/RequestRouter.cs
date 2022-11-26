using VacationAPI.Context;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Services.Validation;

namespace VacationAPI.Services.RequestServices;

public class RequestRouter
{
	//@TODO добавить возможность перемещать сотрудников из одной команды в другую
	//@TODO получение количества отпусков на данный момент
	public void Route(WebApplication app)
	{
		//регистрация пользователя. Добавление в бд
		app.MapPost("/api/auth/sign-in/{username}/{password}", (ApplicationContext db, UserValidator userValidator, ILogger<RequestRouter> logger, string username, string password) =>
		{
			return Request.Authentication.Post.User.AddNewUser(db, userValidator, logger, username, password);
		});

		//получение токена
		app.MapGet("/api/auth/token/{username}/{password}", (ApplicationContext db, ILogger<RequestRouter> logger, string username, string password) =>
		{
			return JwtToken.GetJwtToken(db, logger, username, password);
		});

		//Создание новой команды
		app.MapPost("api/teams/new-team/{teamName}/{username}/{accessToken}",
			(ApplicationContext db, TeamValidator teamValidator, ILogger<RequestRouter> logger, string teamName, string username, string accessToken) =>
			{
				return Request.Teams.Post.Team.AddNewTeam(db, teamValidator, logger, teamName, username, accessToken);
			});

		//Получение списка всех команд пользователя
		app.MapGet("api/teams/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string username, string accessToken) =>
			{
				return Request.Teams.Get.Teams.GetTeams(db, logger, username, accessToken);
			});

		//Получение диаграммы с количеством отпусков всех команд за год по месяцам
		app.MapGet("api/teams/stats-by-months/all-teams/vacations-count/per-year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string year, string username, string accessToken) =>
			{
				return Request.Teams.Get.YearTeamsStats.GetTeamsStats(db, logger, year, username, accessToken);
			});

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

		//Получение диаграммы с количеством отпусков конкретной команды за год по месяцам
		app.MapGet("api/teams/stats-by-months/team/{teamName}/vacations-count/current/per-year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string year, string username, string accessToken) =>
			{
				return Request.Teams.Get.YearTeamStats.GetTeamsStats(db, logger, teamName, year, username,
					accessToken);
			});

		// //Получение диаграммы с количеством всех запланированных отпусков
		// app.MapGet("api/teams/stats/team/vacations-count/planned/{teamName}/{username}/{accessToken}",
		// 	(ApplicationContext db, string teamName, string username, string accessToken) =>
		// 	{
		// 		return VacationAPI.Request.Teams.Get.TeamStats.PlannedVacations.GetTeamStats(db, logger, teamName, username, accessToken);
		// 	});

		//Изменение название команды
		app.MapPut("api/teams/team/{teamName}/{newTeamName}/{username}/{accessToken}",
			(ApplicationContext db, TeamValidator teamValidator, ILogger<RequestRouter> logger, string teamName, string newTeamName, string username,
			string accessToken) =>
			{
				return Request.Teams.Put.TeamName.EditTeamName(db, teamValidator, logger, teamName, newTeamName, username,
					accessToken);
			});

		//удаление команды
		app.MapDelete("api/teams/team/{teamName}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string username, string accessToken) =>
			{
				return Request.Teams.Delete.Team.RemoveTeam(db, logger, teamName, username, accessToken);
			});

		//добавление работника
		app.MapPost("api/teams/team/{teamName}/employees/new-employee/{employeeName}/{username}/{accessToken}",
			(ApplicationContext db, EmployeeValidator employeeValidator, ILogger<RequestRouter> logger, string teamName, string employeeName, string username,
			string accessToken) =>
			{
				return Request.Employees.Post.Employee.AddNewEmployee(db, employeeValidator, logger, teamName, employeeName, username,
					accessToken);
			});

		//Получение диаграммы с количеством отпусков конкретного работника за каждый месяц года
		app.MapGet("api/teams/stats-by-months/team/{teamName}/employee/{employeeName}/per-year/{year}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string year, string username,
			string accessToken) =>
			{
				return Request.Employees.Get.YearEmployeeStats.GetTeamsStats(db, logger, teamName, employeeName, year,
					username, accessToken);
			});

		//получение списка работников команды
		app.MapGet("api/teams/{teamName}/employees/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string username,
			string accessToken) =>
			{
				return Request.Employees.Get.Employees.GetEmployees(db, logger, teamName, username, accessToken);
			});

		//изменение имени работника
		app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeeName}/{username}/{accessToken}",
			(ApplicationContext db, EmployeeValidator employeeValidator, ILogger<RequestRouter> logger, string teamName, string employeeName, string newEmployeeName,
			string username,
			string accessToken) =>
			{
				return Request.Employees.Put.EmployeeName.EditEmployeeName(db, employeeValidator, logger, teamName, employeeName, newEmployeeName,
					username,
					accessToken);
			});

		//удаление работника
		app.MapDelete("api/teams/{teamName}/employees/employee/{employeeName}/{username}/{accessToken}",
			(ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string username,
			string accessToken) =>
			{
				return Request.Employees.Delete.Employee.RemoveEmployee(db, logger, teamName, employeeName, username,
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
		//список отпусков работника
		app.MapGet("api/teams/{teamName}/employees/{employeeName}/vacations-count/{username}/{accessToken}", (
			ApplicationContext db, ILogger<RequestRouter> logger, string teamName, string employeeName, string username,
			string accessToken) =>
		{
			return Request.Vacations.Get.Employee.GetVacations(db, logger, teamName, employeeName, username,
				accessToken);
		});

		//изменение начала отпуска сотрудника
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

		//изменение конца отпуска сотрудника
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

		//удаление отпуска сотрудника
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