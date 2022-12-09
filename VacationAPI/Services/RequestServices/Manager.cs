using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using VacationAPI.Contexts;

namespace VacationAPI.Services.RequestServices;

public static class Manager
{
	//Проверка данных запросов пользователя
	public static IResult? CheckRequest(ApplicationContext db, ILogger logger, string username, string accessToken, string newUsername = "",
										string teamName = "",
										string newTeamName = "", string employeeName = "", string newEmployeeName = "",
										string vacationDateStart = "", string newVacationDateStart = "",
										string vacationDateEnd = "",
										string newVacationDateEnd = "", string year = "", string month = "", string day = "")
	{
		JwtSecurityToken jwtSecurityToken; //JWT токен
		DateOnly vacationStart; //Дата начала отпуска
		DateOnly vacationEnd; //Дата конца отпуска
		DateOnly startDate;	//Дата начала отпуска
		DateOnly endDate; //Дата окончания отпуска

		//Получение данных из базы данных
		var user = db.Users.Include(x => x.Teams.Where(y => y.Name == teamName || y.Name == newTeamName))
			.ThenInclude(x => x.Employees.Where(y => y.Name == employeeName || y.Name == newEmployeeName))
			.FirstOrDefault(x => x.Name == username);

		//Проверка JWT токена
		try
		{
			jwtSecurityToken = new(accessToken);
		}
		catch (Exception)
		{
			logger.LogWarning("Request error: invalid accessToken");

			//Ответ в случае ошибки
			return Results.Json("Неверный accessToken");
		}

		//Проверка наличия пользователя в базе данных
		if (user == null)
		{
			logger.LogWarning("Request error: user not found");

			//Ответ в случае ошибки
			return Results.Json("Пользователь не найден");
		}

		//Проверка на совпадение имен при добавлении нового пользователя
		if (newUsername != "" && db.Users.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: user with this name already exists");

			//Ответ в случае ошибки
			return Results.Json("Пользователь с таким именем уже существует");
		}

		//Проверка JWT токена
		if (jwtSecurityToken.Claims.First()
				.Value
			!= username)
		{
			logger.LogWarning("Request error: invalid accessToken");

			//Ответ в случае ошибки
			return Results.Json("Неверный accessToken");
		}

		//Проверка на наличие команды в базе данных
		if (teamName != "" && user.Teams.FirstOrDefault() == null)
		{
			logger.LogWarning("Request error: team not found");

			//Ответ в случае ошибки
			return Results.Json("Команда не найдена");
		}

		//Проверка на совпадение имен при добавлении команды/смены имени команды
		if (newTeamName != "" && user.Teams.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: team with this name already exists");

			//Ответ в случае ошибки
			return Results.Json("Такая команда уже существует");
		}

		//Проверка на наличие сотрудника в базе данных
		if (employeeName != ""
			&& user.Teams.FirstOrDefault()!
				.Employees.FirstOrDefault()
			== null)
		{
			logger.LogWarning("Request error: employee not found");

			//Ответ в случае ошибки
			return Results.Json("Сотрудник не найден");
		}

		//Проверка на совпадение имен при добавлении в команду/смены имени сотрудника
		if (newEmployeeName != ""
			&& user.Teams.FirstOrDefault()!.Employees.FirstOrDefault() != null)
		{
			logger.LogWarning("Request error: employee with this name already exists");

			//Ответ в случае ошибки
			return Results.Json("Сотрудник с таким именем уже существует в данной команде");
		}

		//Проверки для уже существующего отпуска
		if (vacationDateStart != "" && vacationDateEnd != "")
		{
			//Проверка на валидность даты
			if (DateOnly.TryParse(vacationDateStart, out vacationStart) && DateOnly.TryParse(vacationDateEnd, out vacationEnd))
			{
				//Проверка на наличие отпуска в базе данных
				if (user.Teams.FirstOrDefault()?
						.Employees.FirstOrDefault()?
						.Vacations.Find(x => x.StartOfVacation == startDate && x.EndOfVacation == endDate)
					== null)

				{
					logger.LogWarning("Request error: vacation not found");

					//Ответ в случае ошибки
					return Results.Json("Отпуск с таким значением даты не найден");
				}

				//Проверки при изменении даты начала или даты окончания отпуска
				if (newVacationDateStart != "" || newVacationDateEnd != "")
				{
					//Проверка на валидность новой даты
					if (DateOnly.TryParse(newVacationDateStart, out startDate) || DateOnly.TryParse(newVacationDateEnd, out endDate))
					{
						//Проверка на ошибку в датах
						if ((startDate > vacationEnd && newVacationDateStart != "")
							|| (endDate < vacationStart && newVacationDateEnd != ""))
						{
							logger.LogWarning("Request error: impossible to put the end of the vacation before it begins");

							//Ответ в случае ошибки
							return Results.Json("Нельзя поставить окончание отпуска до его начала");
						}
					} else
					{
						logger.LogWarning("Request error: invalid date format");

						//Ответ в случае ошибки
						return Results.Json("Неверный формат даты");
					}
				}
			} else
			{
				logger.LogWarning("Request error: invalid date format");

				//Ответ в случае ошибки
				return Results.Json("Неверный формат даты");
			}
		}

		//Проверки при добавлении нового отпуска
		if (newVacationDateStart != "" && newVacationDateEnd != "")
		{
			//Проверка валидности дат
			if (DateOnly.TryParse(newVacationDateStart, out startDate) && DateOnly.TryParse(newVacationDateEnd, out endDate))
			{
				//Проверка на совпадение с уже существующим отпуском сотрудника
				if (db.Vacations.FirstOrDefault(x => x.StartOfVacation == startDate
													&& x.EndOfVacation == endDate
													&& x.Employee
													== user.Teams.FirstOrDefault()!
														.Employees.FirstOrDefault())
					!= null)
				{
					logger.LogWarning("Request error: vacation with this date already exists");

					//Ответ в случае ошибки
					return Results.Json("Отпуск с таким значением даты уже существует");
				}

				//Проверка на ошибку в датах
				if (startDate > endDate)
				{
					logger.LogWarning("Request error: impossible to put the end of the vacation before it begins");

					//Ответ в случае ошибки
					return Results.Json("Нельзя поставить окончание отпуска до его начала");
				}
			} else
			{
				logger.LogWarning("Request error: invalid date format");

				//Ответ в случае ошибки
				return Results.Json("Неверный формат даты");
			}
		}

		//Проверка валидности введенного года
		if (year != "" && !DateOnly.TryParse("01.01." + year, out var _))
		{
			//Ответ в случае ошибки
			return Results.Json("Неверный формат даты");
		}

		//Проверка валидности введенного месяца
		if (month != "" && !DateOnly.TryParse("01." + month + ".2022", out var _))
		{
			//Ответ в случае ошибки
			return Results.Json("Неверный формат даты");
		}

		//Проверка валидности введенного дня
		if (day != "" && !DateOnly.TryParse(day + ".01.2022", out var _))
		{
			//Ответ в случае ошибки
			return Results.Json("Неверный формат даты");
		}

		//Если в запросе нет никаких ошибок, то возвращает null
		return null;
	}
}