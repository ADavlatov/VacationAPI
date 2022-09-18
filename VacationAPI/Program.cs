using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VacationAPI.Authentication;
using VacationAPI.Context;
using VacationAPI.Entities;
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

app.MapPost("/api/auth/sign-in/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	if (db.Users.FirstOrDefault(x => x.Name == username) != null)
	{
		db.Users.Add(new()
		{
			Name = username,
			Password = MD5Hash.GetHashedString(password)
		});

		db.SaveChanges();

		return Results.Json($"Ваш логин: {username}, Ваш пароль: {password}");
	}

	return Results.Json($"Пользователь с ником {username} уже сущесвует");
});

app.MapPost("/api/auth/log-in/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	var response = new
	{
		access_token = new JwtSecurityTokenHandler().WriteToken(JwtToken.GenerateToken(db, username, MD5Hash.GetHashedString(password))),
		name = username
	};

	return Results.Json(response);
});

app.MapPost("api/teams/newteam/{teamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username, string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token) && db.Users.FirstOrDefault(x => x.Name == username) != null)
		{
			db.Teams.Add(new()
			{
				User = db.Users.FirstOrDefault(x => x.Name == username),
				Name = teamName
			});

			db.SaveChanges();
		}
	});

app.MapPost("api/teams/removeteam/{teamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username, string access_token) =>
	{
		if (JwtToken.CheckJwtToken(username, access_token) && db.Users.FirstOrDefault(x => x.Name == username) != null)
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
		}
	});

app.MapPost("api/teams/{teamName}/employees/{employeeName}/vacations/newvacation/{vacationDateStart}/{vacationDateEnd}", (
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
	}
});

app.Run();