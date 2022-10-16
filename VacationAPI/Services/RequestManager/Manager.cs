using System.IdentityModel.Tokens.Jwt;
using VacationAPI.Context;

namespace VacationAPI.Services.RequestManager;

public class Manager
{
	public static IResult? CheckRequest(ApplicationContext db, ILogger logger, string username, string accessToken, string newUsername = "",
										string teamName = "",
										string newTeamName = "", string employeeName = "", string newEmployeeName = "",
										string vacationDateStart = "", string newVacationDateStart = "",
										string vacationDateEnd = "",
										string newVacationDateEnd = "")
	{
		JwtSecurityToken jwtSecurityToken;
		DateOnly vacationStart;
		DateOnly vacationEnd;
		DateOnly startDate;
		DateOnly endDate;

		Entities.User? user = db.Users.FirstOrDefault(x => x.Name == username);
		Entities.Team? team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User == user);
		Entities.Employee? employee = db.Employees.FirstOrDefault(x => x.Name == employeeName && x.Team == team);

		Entities.User? newUser = db.Users.FirstOrDefault(x => x.Name == newUsername);
		Entities.Team? newTeam = db.Teams.FirstOrDefault(x => x.Name == newTeamName && x.User == user);

		Entities.Employee? newEmployee =
			db.Employees.FirstOrDefault(x => x.Name == newEmployeeName && x.Team == team);

		try
		{
			jwtSecurityToken = new(accessToken);
		}
		catch (Exception)
		{
			logger.LogError("Request error: invalid accessToken");

			return Results.Json("Неверный accessToken");
		}

		if (user == null)
		{
			logger.LogError("Request error: user not found");

			return Results.Json("Пользователь не найден");
		}

		if (newUser != null && newUsername != "")
		{
			logger.LogError("Request error: user with this name already exists");

			return Results.Json("Пользователь с таким именем уже существует");
		}

		if (jwtSecurityToken.Claims.First()
				.Value
			!= username)
		{
			logger.LogError("Request error: invalid accessToken");

			return Results.Json("Неверный accessToken");
		}

		if (team == null && teamName != "")
		{
			logger.LogError("Request error: team not found");

			return Results.Json("Команда не найдена");
		}

		if (newTeam != null && newTeamName != "")
		{
			logger.LogError("Request error: team with this name already exists");

			return Results.Json("Такая команда уже существует");
		}

		if (employee == null && employeeName != "")
		{
			logger.LogError("Request error: employee not found");

			return Results.Json("Сотрудник не найден");
		}

		if (newEmployee != null && newEmployeeName != "")
		{
			logger.LogError("Request error: employee with this name already exists");

			return Results.Json("Сотрудник с таким именем уже существует в данной команде");
		}

		if (vacationDateStart != "" && vacationDateEnd != "")
		{
			if (DateOnly.TryParse(vacationDateStart, out vacationStart) && DateOnly.TryParse(vacationDateEnd, out vacationEnd))
			{
				if (db.Vacations.FirstOrDefault(x =>
						x.StartOfVacation == vacationStart && x.EndOfVacation == vacationEnd && x.Employee == employee)
					== null)
				{
					logger.LogError("Request error: vacation not found");

					return Results.Json("Отпуск с таким значением даты не найден");
				}

				if (newVacationDateStart != "" || newVacationDateEnd != "")
				{
					if (DateOnly.TryParse(newVacationDateStart, out startDate) || DateOnly.TryParse(newVacationDateEnd, out endDate))
					{
						if ((startDate > vacationEnd && newVacationDateStart != "")
							|| (endDate < vacationStart && newVacationDateEnd != ""))
						{
							logger.LogError("Request error: impossible to put the end of the vacation before it begins");

							return Results.Json("Нельзя поставить окончание отпуска до его начала");
						}
					} else
					{
						logger.LogError("Request error: invalid date format");

						return Results.Json("Неверный формат даты");
					}
				}
			} else
			{
				logger.LogError("Request error: invalid date format");

				return Results.Json("Неверный формат даты");
			}
		}

		if (newVacationDateStart != "" && newVacationDateEnd != "")
		{
			if (DateOnly.TryParse(newVacationDateStart, out startDate) && DateOnly.TryParse(newVacationDateEnd, out endDate))
			{
				if (db.Vacations.FirstOrDefault(x => x.StartOfVacation == startDate && x.EndOfVacation == endDate && x.Employee == employee)
					!= null)
				{
					logger.LogError("Request error: vacation with this date already exists");

					return Results.Json("Отпуск с таким значением даты уже существует");
				}

				if (startDate > endDate)
				{
					logger.LogError("Request error: impossible to put the end of the vacation before it begins");

					return Results.Json("Нельзя поставить окончание отпуска до его начала");
				}
			} else
			{
				logger.LogError("Request error: invalid date format");

				return Results.Json("Неверный формат даты");
			}
		}

		return null;
	}
}