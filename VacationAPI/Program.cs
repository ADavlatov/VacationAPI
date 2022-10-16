using Microsoft.AspNetCore.Authentication.JwtBearer;
using VacationAPI.Context;
using VacationAPI.Request.Authentication;
using VacationAPI.Request.Authentication.Get;

var builder = WebApplication.CreateBuilder(args);

var logger = LoggerFactory.Create(config =>
	{
		config.AddConsole();
	})
	.CreateLogger("Program");

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new()
		{
			ValidateIssuer = true,
			ValidIssuer = AuthOptions.ISSUER,
			ValidateAudience = true,
			ValidAudience = AuthOptions.AUDIENCE,
			ValidateLifetime = true,
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//@TODO добавить получение диаграмм со статистикой через https://github.com/image-charts/c-sharp
//@TODO добавить возможность перемещать сотрудников из одной команды в другую
//@TODO вернуть fluentvalidator

//регистрация пользователя. Добавление в бд
app.MapPost("/api/auth/sign-in/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	return VacationAPI.Authentication.Post.User.AddNewUser(db, logger, username, password);
});

//получение токена
app.MapGet("/api/auth/token/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	return JwtToken.GetJwtToken(db, logger, username, password);
});

//Создание новой команды
app.MapPost("api/teams/new-team/{teamName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username, string accessToken) =>
	{
		return VacationAPI.Request.Teams.Post.Team.AddNewTeam(db, logger, teamName, username, accessToken);
	});

//Получение списка всех команд пользователя
app.MapGet("api/teams/{username}/{accessToken}",
	(ApplicationContext db, string username, string accessToken) =>
	{
		return VacationAPI.Request.Teams.Get.Teams.GetTeams(db, logger, username, accessToken);
	});

//Получение диаграммы со статистикой всех команд (одна диаграмма для всех команд)
app.MapGet("api/teams/general/stats/{username}/{accessToken}",
	(ApplicationContext db, string username, string accessToken) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Получение диаграмм со статистикой всех команд (диаграмма для каждой команды)
app.MapGet("api/teams/stats/{username}/{accessToken}",
	(ApplicationContext db, string username, string accessToken) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Получение статистики отдельной команды
app.MapGet("api/teams/team/{teamName}/stats/{username}/{accessToken}",
	(ApplicationContext db, string username, string accessToken) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Изменение название команды
app.MapPut("api/teams/team/{teamName}/{newTeamName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string newTeamName, string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Teams.Put.TeamName.EditTeamName(db, logger, teamName, newTeamName, username,
			accessToken);
	});

//удаление команды
app.MapDelete("api/teams/team/{teamName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username, string accessToken) =>
	{
		return VacationAPI.Request.Teams.Delete.Team.RemoveTeam(db, logger, teamName, username, accessToken);
	});

//добавление работника
app.MapPost("api/teams/{teamName}/employees/new-employee/{employeeName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string employeeName, string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Employees.Post.Employee.AddNewEmployee(db, logger, teamName, employeeName, username,
			accessToken);
	});

//получение списка работников команды
app.MapGet("api/teams/{teamName}/employees/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Employees.Get.Employees.GetEmployees(db, logger, teamName, username, accessToken);
	});

//получение диаграмм со статистикой всех работников команды
app.MapGet("api/teams/{teamName}/employees/stats/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username,
	string accessToken) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//получение статистики конкретного работника
app.MapGet("api/teams/{teamName}/employees/employee/{employeeName}/stats/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username,
	string accessToken) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//изменение имени работника
app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeeName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string employeeName, string newEmployeeName,
	string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Employees.Put.EmployeeName.EditEmployeeName(db, logger, teamName, employeeName, newEmployeeName,
			username,
			accessToken);
	});

//удаление работника
app.MapDelete("api/teams/{teamName}/employees/employee/{employeeName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string employeeName, string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Employees.Delete.Employee.RemoveEmployee(db, logger, teamName, employeeName, username,
			accessToken);
	});

//Добавление отпуска сотруднику
app.MapPost(
	"api/teams/{teamName}/employees/{employeeName}/vacations/new-vacation/{vacationDateStart}/{vacationDateEnd}/{username}/{accessToken}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
		string vacationDateEnd,
		string username,
		string accessToken) =>
	{
		return VacationAPI.Request.Vacations.Post.Vacation.AddNewVacation(db, logger, teamName, employeeName, vacationDateStart,
			vacationDateEnd,
			username, accessToken);
	});

//@TODO сделать вывод даты начала и даты окончания каждого отпуска
//список отпусков работника
app.MapGet("api/teams/{teamName}/employees/{employeeName}/vacations-count/{username}/{accessToken}", (
	ApplicationContext db, string teamName, string employeeName, string username,
	string accessToken) =>
{
	return VacationAPI.Request.Vacations.Get.Employee.GetVacations(db, logger, teamName, employeeName, username,
		accessToken);
});

//изменение начала отпуска сотрудника
app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/vacationDateStart/{newDate}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
	string vacationDateEnd,
	string newDate,
	string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Vacations.Put.VacationStart.EditVacationStart(db, logger, teamName, employeeName, vacationDateStart,
			vacationDateEnd,
			newDate, username, accessToken);
	});

//изменение конца отпуска сотрудника
app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/vacationDateEnd/{newDate}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
	string vacationDateEnd,
	string newDate, string username,
	string accessToken) =>
	{
		return VacationAPI.Request.Vacations.Put.VacationEnd.EditVacationEnd(db, logger, teamName, employeeName, vacationDateStart,
			vacationDateEnd,
			newDate, username, accessToken);
	});

//удаление отпуска сотрудника
app.MapDelete(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/{username}/{accessToken}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart,
		string vacationDateEnd,
		string username,
		string accessToken) =>
	{
		return VacationAPI.Request.Vacations.Delete.Vacation.RemoveVacation(db, logger, teamName, employeeName, vacationDateStart,
			vacationDateEnd,
			username,
			accessToken);
	});

app.Run();