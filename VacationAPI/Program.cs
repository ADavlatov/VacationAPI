using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using VacationAPI.Authentication;
using VacationAPI.Authentication.Post;
using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Queries.Employees.Get;
using VacationAPI.Queries.Employees.Post;
using VacationAPI.Queries.Employees.Put;
using VacationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

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

//@TODO подключить fluent validator
//@TODO добавить получение диаграмм со статистикой через https://github.com/image-charts/c-sharp

//регистрация пользователя. Добавление в бд
app.MapPost("/api/auth/sign-in/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	return NewUser.AddNewUser(db, username, password);
});

//получение токена
app.MapGet("/api/auth/token/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	return JwtToken.GetJwtToken(db, username, password);
});

//Создание новой команды
app.MapPost("api/teams/newteam/{teamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username, string access_token) =>
	{
		return NewTeam.AddNewTeam(db, teamName, username, access_token);
	});

//Получение списка всех команд пользователя
app.MapGet("api/teams/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		return Teams.GetTeams(db, username, access_token);
	});

//Получение диаграммы со статистикой команды
app.MapGet("api/teams/stats/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{

	});

//Получение статистики отдельной команды
app.MapGet("api/teams/team/{teamName}/stats/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token) && db.Users.FirstOrDefault(x => x.Name == username) != null)
		{
		}
	});

//Изменение название команды
app.MapPut("api/teams/team/{teamName}/{newTeamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string newTeamName, string username, string access_token) =>
	{
		return TeamName.EditTeamName(db, teamName, newTeamName, username, access_token);
	});

app.MapDelete("api/teams/team/{teamName}/{username}/{accessToken}",
	(ApplicationContext db, string teamName, string username, string accessToken) =>
	{
		if (JwtToken.CheckJwtToken(username, accessToken) && db.Users.FirstOrDefault(x => x.Name == username) != null)
		{
			db.Teams.Remove(db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName));

			db.SaveChanges();
		}
	});

app.MapPost("api/teams/{teamName}/employees/newemployee/{employeeName}/{employeePosition}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string employeePosition, string username,
	string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token)
			&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null)
		{
			db.Employees.Add(new()
			{
				Team = db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName)!,
				Name = employeeName,
				Position = employeePosition
			});

			db.SaveChanges();
		}
	});

app.MapGet("api/teams/{teamName}/employees/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username,
	string access_token) =>
	{
	});

app.MapGet("api/teams/{teamName}/employees/employee/{employeeName}/stats/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username,
	string access_token) =>
	{
	});

app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeeName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string newEmployeeName, string username,
	string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token)
			&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null)
		{
		}
	});

app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeePosition}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string newEmployeePosition, string username,
	string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token)
			&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null)
		{
		}
	});

app.MapDelete("api/teams/{teamName}/employees/employee/{employeeName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string username,
	string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token)
			&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null)
		{
			db.Employees.Remove(db.Employees.FirstOrDefault(x => x.Team.Name == teamName && x.Name == employeeName));
			db.SaveChanges();
		}
	});

app.MapPost(
	"api/teams/{teamName}/employees/{employeeName}/vacations/newvacation/{vacationDateStart}/{vacationDateEnd}/{username}/{access_token}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string vacationDateEnd,
		string username,
		string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token)
			&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null
			&& db.Employees.FirstOrDefault(x => x.Name == employeeName) != null)
		{
			db.Vacations.Add(new()
			{
				Employee = db.Employees.FirstOrDefault(x => x.Name == employeeName),
				StartOfVacation = DateOnly.Parse(vacationDateStart),
				EndOfVacation = DateOnly.Parse(vacationDateEnd)
			});

			db.SaveChanges();
		}
	});

app.MapGet("api/teams/{teamName}/employees/{employeeName}/vacations/{username}/{access_token}", (
	ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
	string access_token) =>
{
	if (JwtToken.CheckJwtToken(username, access_token))
	{
	}
});

app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{newVacationDateStart}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
	string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token))
		{
		}
	});

app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{newVacationDateEnd}/{username}/{access_token}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
		string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token))
		{
		}
	});

app.MapDelete(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{vacationDateEnd}/{username}/{access_token}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
		string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token))
		{
			db.Vacations.Remove(db.Vacations.FirstOrDefault(x =>
				x.Employee.Name == employeeName
				&& x.StartOfVacation == DateOnly.Parse(vacationDateStart)
				&& db.Teams.FirstOrDefault(x => x.User.Name == username && x.Name == teamName) != null)!);

			db.SaveChanges();
		}
	});

app.Run();