using System.IdentityModel.Tokens.Jwt;
using VacationAPI.Context;

namespace VacationAPI.Services.RequestManager;

public class Manager
{
	public static IResult? CheckRequest(ApplicationContext db, string username, string accessToken, string newUsername = "",
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
			return Results.Json("Неверный accessToken");
		}

		if (user == null)
		{
			return Results.Json("Пользователь не найден");
		}

		if (newUser != null && newUsername != "")
		{
			return Results.Json("Пользователь с таким именем уже существует");
		}

		if (jwtSecurityToken.Claims.First()
				.Value
			!= username)
		{
			return Results.Json("Неверный accessToken");
		}

		if (team == null && teamName != "")
		{
			return Results.Json("Команда не найдена");
		}

		if (newTeam != null && newTeamName != "")
		{
			return Results.Json("Такая команда уже существует");
		}

		if (employee == null && employeeName != "")
		{
			return Results.Json("Сотрудник не найден");
		}

		if (newEmployee != null && newEmployeeName != "")
		{
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
					return Results.Json("Отпуск с таким значением даты не найден");
				}

				if (newVacationDateStart != "" || newVacationDateEnd != "")
				{
					if (DateOnly.TryParse(newVacationDateStart, out startDate) || DateOnly.TryParse(newVacationDateEnd, out endDate))
					{
						if ((startDate > vacationEnd && newVacationDateStart != "")
							|| (endDate < vacationStart && newVacationDateEnd != ""))
						{
							return Results.Json("Нельзя поставить окончание отпуска до его начала");
						}
					} else
					{
						return Results.Json("Неверный формат даты");
					}
				}
			} else
			{
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
					return Results.Json("Отпуск с таким значением даты уже существует");
				}

				if (startDate > endDate)
				{
					return Results.Json("Нельзя поставить окончание отпуска до его начала");
				}
			} else
			{
				return Results.Json("Неверный формат даты");
			}
		}

		return null;
	}
}