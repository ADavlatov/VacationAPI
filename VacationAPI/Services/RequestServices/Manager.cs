using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using VacationAPI.Context;

namespace VacationAPI.Services.RequestServices;

public static class Manager
{
	public static IResult? CheckRequest(ApplicationContext db, ILogger logger, string username, string accessToken, string newUsername = "",
										string teamName = "",
										string newTeamName = "", string employeeName = "", string newEmployeeName = "",
										string vacationDateStart = "", string newVacationDateStart = "",
										string vacationDateEnd = "",
										string newVacationDateEnd = "", string year = "")
	{
		JwtSecurityToken jwtSecurityToken;
		DateOnly vacationStart;
		DateOnly vacationEnd;
		DateOnly startDate;
		DateOnly endDate;

		var user = db.Users.Include(x => x.Teams.Where(y => y.Name == teamName || y.Name == newTeamName))
			.ThenInclude(x => x.Employees.Where(y => y.Name == employeeName || y.Name == newEmployeeName))
			.FirstOrDefault(x => x.Name == username);

		try
		{
			jwtSecurityToken = new(accessToken);
		}
		catch (Exception)
		{
			logger.LogWarning("Request error: invalid accessToken");

			return Results.Json("Неверный accessToken");
		}

		if (user == null)
		{
			logger.LogWarning("Request error: user not found");

			return Results.Json("Пользователь не найден");
		}

		if (newUsername != "" && db.Users.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: user with this name already exists");

			return Results.Json("Пользователь с таким именем уже существует");
		}

		if (jwtSecurityToken.Claims.First()
				.Value
			!= username)
		{
			logger.LogWarning("Request error: invalid accessToken");

			return Results.Json("Неверный accessToken");
		}

		if (teamName != "" && user.Teams.FirstOrDefault() == null)
		{
			logger.LogWarning("Request error: team not found");

			return Results.Json("Команда не найдена");
		}

		if (newTeamName != "" && user.Teams.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: team with this name already exists");

			return Results.Json("Такая команда уже существует");
		}

		if (employeeName != ""
			&& user.Teams.FirstOrDefault()!
				.Employees.FirstOrDefault()
			== null)
		{
			logger.LogWarning("Request error: employee not found");

			return Results.Json("Сотрудник не найден");
		}

		if (newEmployeeName != ""
			&& user.Teams.FirstOrDefault()!.Employees.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: employee with this name already exists");

			return Results.Json("Сотрудник с таким именем уже существует в данной команде");
		}

		if (vacationDateStart != "" && vacationDateEnd != "")
		{
			if (DateOnly.TryParse(vacationDateStart, out vacationStart) && DateOnly.TryParse(vacationDateEnd, out vacationEnd))
			{
				if (user.Teams.FirstOrDefault()
						.Employees.FirstOrDefault()
						.Vacations.Find(x => x.StartOfVacation == startDate && x.EndOfVacation == endDate)
					== null)

				{
					logger.LogWarning("Request error: vacation not found");

					return Results.Json("Отпуск с таким значением даты не найден");
				}

				if (newVacationDateStart != "" || newVacationDateEnd != "")
				{
					if (DateOnly.TryParse(newVacationDateStart, out startDate) || DateOnly.TryParse(newVacationDateEnd, out endDate))
					{
						if ((startDate > vacationEnd && newVacationDateStart != "")
							|| (endDate < vacationStart && newVacationDateEnd != ""))
						{
							logger.LogWarning("Request error: impossible to put the end of the vacation before it begins");

							return Results.Json("Нельзя поставить окончание отпуска до его начала");
						}
					} else
					{
						logger.LogWarning("Request error: invalid date format");

						return Results.Json("Неверный формат даты");
					}
				}
			} else
			{
				logger.LogWarning("Request error: invalid date format");

				return Results.Json("Неверный формат даты");
			}
		}

		if (newVacationDateStart != "" && newVacationDateEnd != "")
		{
			if (DateOnly.TryParse(newVacationDateStart, out startDate) && DateOnly.TryParse(newVacationDateEnd, out endDate))
			{
				if (db.Vacations.FirstOrDefault(x => x.StartOfVacation == startDate
													&& x.EndOfVacation == endDate
													&& x.Employee
													== user.Teams.FirstOrDefault()!
														.Employees.FirstOrDefault())
					!= null)
				{
					logger.LogWarning("Request error: vacation with this date already exists");

					return Results.Json("Отпуск с таким значением даты уже существует");
				}

				if (startDate > endDate)
				{
					logger.LogWarning("Request error: impossible to put the end of the vacation before it begins");

					return Results.Json("Нельзя поставить окончание отпуска до его начала");
				}
			} else
			{
				logger.LogWarning("Request error: invalid date format");

				return Results.Json("Неверный формат даты");
			}
		}

		if (year != "" && !DateOnly.TryParse("01.01." + year, out var _))
		{
			return Results.Json("Неверный формат даты");
		}

		return null;
	}
}