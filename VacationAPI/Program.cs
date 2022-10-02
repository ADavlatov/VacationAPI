using Microsoft.AspNetCore.Authentication.JwtBearer;
using VacationAPI.Context;
using VacationAPI.Entities;
using VacationAPI.Request;
using VacationAPI.Request.Authentication;
using VacationAPI.Request.Authentication.Get;
using VacationAPI.Validation;
using VacationAPI.Validation.ValidationParameters;

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

//@TODO проверить все ли ошибки вводных данных учтены
//@TODO добавить получение диаграмм со статистикой через https://github.com/image-charts/c-sharp
//@TODO добавить возможность перемещать сотрудников из одной команды в другую
//@TODO убрать employeePosition отовсюду

RequestValidator _requestValidator;
EmployeeValidator _employeeValidator;
TeamValidator _teamValidator;
DateValidator _dateValidator;

//регистрация пользователя. Добавление в бд
app.MapPost("/api/auth/sign-in/{username}/{password}", (ApplicationContext db, string username, string password) =>
{
	return VacationAPI.Authentication.Post.User.AddNewUser(db, username, password);
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
		_requestValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		if (request.IsValid)
		{
			return VacationAPI.Request.Teams.Post.NewTeam.AddNewTeam(db, teamName, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors)
		};

		return Results.Json(errorsResponse);
	});

//Получение списка всех команд пользователя
app.MapGet("api/teams/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		_requestValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		if (request.IsValid)
		{
			return VacationAPI.Request.Teams.Get.Teams.GetTeams(db, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors)
		};

		return Results.Json(errorsResponse);
	});

//Получение диаграммы со статистикой всех команд (одна диаграмма для всех команд)
app.MapGet("api/teams/general/stats/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Получение диаграмм со статистикой всех команд (диаграмма для каждой команды)
app.MapGet("api/teams/stats/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Получение статистики отдельной команды
app.MapGet("api/teams/team/{teamName}/stats/{username}/{access_token}",
	(ApplicationContext db, string username, string access_token) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//Изменение название команды
app.MapPut("api/teams/team/{teamName}/{newTeamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string newTeamName, string username, string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		if (request.IsValid && team.IsValid)
		{
			return VacationAPI.Request.Teams.Put.TeamName.EditTeamName(db, teamName, newTeamName, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors)
		};

		return Results.Json(errorsResponse);
	});

//удаление команды
app.MapDelete("api/teams/team/{teamName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username, string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		if (request.IsValid && team.IsValid)
		{
			return VacationAPI.Request.Teams.Delete.Team.RemoveTeam(db, teamName, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors)
		};

		return Results.Json(errorsResponse);
	});

//добавление работника
app.MapPost("api/teams/{teamName}/employees/newemployee/{employeeName}/{employeePosition}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string employeePosition, string username,
	string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		if (request.IsValid && team.IsValid)
		{
			return VacationAPI.Request.Employees.Post.Employee.AddNewEmployee(db, teamName, employeeName, employeePosition, username,
				access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors)
		};

		return Results.Json(errorsResponse);
	});

//получение списка работников команды
app.MapGet("api/teams/{teamName}/employees/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username,
	string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		if (request.IsValid && team.IsValid)
		{
			return VacationAPI.Request.Employees.Get.Employees.GetEmployees(db, teamName, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors)
		};

		return Results.Json(errorsResponse);
	});

//получение диаграмм со статистикой всех работников команды
app.MapGet("api/teams/{teamName}/employees/stats/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username,
	string access_token) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//получение статистики конкретного работника
app.MapGet("api/teams/{teamName}/employees/employee/{employeeName}/stats/{username}/{access_token}",
	(ApplicationContext db, string teamName, string username,
	string access_token) =>
	{
		//@TODO добавить диаграммы и доделать
	});

//изменение имени работника
app.MapPut("api/teams/{teamName}/employees/employee/{employeeName}/{newEmployeeName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string newEmployeeName, string username,
	string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);
		_employeeValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		var employee = _employeeValidator.Validate(new EmployeeNameParameter
		{
			EmployeeName = employeeName
		});

		if (request.IsValid && team.IsValid && employee.IsValid)
		{
			return VacationAPI.Request.Employees.Put.EmployeeName.EditEmployeeName(db, teamName, employeeName, newEmployeeName, username,
				access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors),
			employeeErrors = string.Join("\r\n", employee.Errors)
		};

		return Results.Json(errorsResponse);
	});

//удаление работника
app.MapDelete("api/teams/{teamName}/employees/employee/{employeeName}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string username,
	string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);
		_employeeValidator = new(db);

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		var employee = _employeeValidator.Validate(new EmployeeNameParameter
		{
			EmployeeName = employeeName
		});

		if (request.IsValid && team.IsValid && employee.IsValid)
		{
			return VacationAPI.Request.Employees.Delete.Employee.RemoveEmployee(db, teamName, employeeName, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors),
			employeeErrors = string.Join("\r\n", employee.Errors)
		};

		return Results.Json(errorsResponse);
	});

//Добавление отпуска сотруднику
app.MapPost(
	"api/teams/{teamName}/employees/{employeeName}/vacations/newvacation/{vacationDateStart}/{vacationDateEnd}/{username}/{access_token}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string vacationDateEnd,
		string username,
		string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);
		_employeeValidator = new(db);
		_dateValidator = new();

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		var employee = _employeeValidator.Validate(new EmployeeNameParameter
		{
			EmployeeName = employeeName
		});

		var dateStart = _dateValidator.Validate(new DateParameter
		{
			VacationDate = vacationDateStart
		});

		var dateEnd = _dateValidator.Validate(new DateParameter
		{
			VacationDate = vacationDateEnd
		});

		if (request.IsValid && team.IsValid && employee.IsValid && dateStart.IsValid && dateEnd.IsValid)
		{
			return VacationAPI.Request.Vacations.Post.Vacation.AddNewVacation(db, teamName, employeeName, vacationDateStart,
				vacationDateEnd,
				username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors),
			employeeErrors = string.Join("\r\n", employee.Errors),
			dateStartErrors = string.Join("\r\n", dateStart.Errors),
			dateEndErrors = string.Join("\r\n", dateEnd.Errors)
		};

		return Results.Json(errorsResponse);
	});

//список отпусков работника
app.MapGet("api/teams/{teamName}/employees/{employeeName}/vacations/{username}/{access_token}", (
	ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
	string access_token) =>
{
	_requestValidator = new(db);
	_teamValidator = new(db);
	_employeeValidator = new(db);

	var request = _requestValidator.Validate(new Request
	{
		Username = username,
		AccessToken = access_token
	});

	var team = _teamValidator.Validate(new TeamNameParameter
	{
		TeamName = teamName
	});

	var employee = _employeeValidator.Validate(new EmployeeNameParameter
	{
		EmployeeName = employeeName
	});

	if (request.IsValid && team.IsValid && employee.IsValid)
	{
		return VacationAPI.Request.Vacations.Get.Employee.GetVacations(db, teamName, employeeName, vacationDateStart, username,
			access_token);
	}

	var errorsResponse = new
	{
		requestErrors = string.Join("\r\n", request.Errors),
		teamErrors = string.Join("\r\n", team.Errors),
		employeeErrors = string.Join("\r\n", employee.Errors)
	};

	return Results.Json(errorsResponse);
});

//изменение начала отпуска сотрудника
app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{newVacationDateStart}/{username}/{access_token}",
	(ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string newVacationDateStart,
	string username,
	string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);
		_employeeValidator = new(db);
		_dateValidator = new();

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		var employee = _employeeValidator.Validate(new EmployeeNameParameter
		{
			EmployeeName = employeeName
		});

		var dateStart = _dateValidator.Validate(new DateParameter
		{
			VacationDate = vacationDateStart
		});

		if (request.IsValid && team.IsValid && employee.IsValid && dateStart.IsValid)
		{
			return VacationAPI.Request.Vacations.Put.VacationStart.EditVacationStart(db, teamName, employeeName, vacationDateStart,
				newVacationDateStart, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors),
			employeeErrors = string.Join("\r\n", employee.Errors),
			dateStartErrors = string.Join("\r\n", dateStart.Errors)
		};

		return Results.Json(errorsResponse);
	});

//изменение конца отпуска сотрудника
app.MapPut(
	"api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{newVacationDateEnd}/{username}/{access_token}", (
		ApplicationContext db, string teamName, string employeeName, string vacationDateEnd,
		string newVacationDateEnd, string username,
		string access_token) =>
	{
		_requestValidator = new(db);
		_teamValidator = new(db);
		_employeeValidator = new(db);
		_dateValidator = new();

		var request = _requestValidator.Validate(new Request
		{
			Username = username,
			AccessToken = access_token
		});

		var team = _teamValidator.Validate(new TeamNameParameter
		{
			TeamName = teamName
		});

		var employee = _employeeValidator.Validate(new EmployeeNameParameter
		{
			EmployeeName = employeeName
		});

		var dateEnd = _dateValidator.Validate(new DateParameter
		{
			VacationDate = vacationDateEnd
		});

		if (request.IsValid && team.IsValid && employee.IsValid && dateEnd.IsValid)
		{
			return VacationAPI.Request.Vacations.Put.VacationEnd.EditVacationEnd(db, teamName, employeeName, vacationDateEnd,
				newVacationDateEnd, username, access_token);
		}

		var errorsResponse = new
		{
			requestErrors = string.Join("\r\n", request.Errors),
			teamErrors = string.Join("\r\n", team.Errors),
			employeeErrors = string.Join("\r\n", employee.Errors),
			dateStartErrors = string.Join("\r\n", dateEnd.Errors)
		};

		return Results.Json(errorsResponse);
	});

//удаление отпуска сотрудника
app.MapDelete("api/teams/{teamName}/employees/{employeeName}/vacations/vacation/{vacationDateStart}/{username}/{access_token}", (
	ApplicationContext db, string teamName, string employeeName, string vacationDateStart, string username,
	string access_token) =>
{
	_requestValidator = new(db);
	_teamValidator = new(db);
	_employeeValidator = new(db);
	_dateValidator = new();

	var request = _requestValidator.Validate(new Request
	{
		Username = username,
		AccessToken = access_token
	});

	var team = _teamValidator.Validate(new TeamNameParameter
	{
		TeamName = teamName
	});

	var employee = _employeeValidator.Validate(new EmployeeNameParameter
	{
		EmployeeName = employeeName
	});

	var dateStart = _dateValidator.Validate(new DateParameter
	{
		VacationDate = vacationDateStart
	});

	if (request.IsValid && team.IsValid && employee.IsValid && dateStart.IsValid)
	{
		return VacationAPI.Request.Vacations.Delete.Vacation.RemoveVacation(db, teamName, employeeName, vacationDateStart, username,
			access_token);

		;
	}

	var errorsResponse = new
	{
		requestErrors = string.Join("\r\n", request.Errors),
		teamErrors = string.Join("\r\n", team.Errors),
		employeeErrors = string.Join("\r\n", employee.Errors),
		dateStartErrors = string.Join("\r\n", dateStart.Errors),
	};

	return Results.Json(errorsResponse);
});

app.Run();