using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using Team = VacationAPI.Entities.Team;

namespace VacationAPI.Request.Employees.Post;

public class Employee
{
	//Добавление нового сотрудника в заданную команду
	public static IResult AddNewEmployee(ApplicationContext db, EmployeeValidator employeeValidator, ILogger logger, string teamName,
										string employeeName,
										string username,
										string accessToken)
	{
		logger.LogInformation("Add new employee: start");

		//Проверка имени сотрудника на валидность
		if (!employeeValidator.Validate(new Services.Validation.Employee(employeeName))
				.IsValid)
		{
			logger.LogError("Add new employee: wrong format of input");

			//Ответ в случае ошибки в имени сотрудника
			return Results.Json("Недопустимый формат ввода. Имя сотрудника должно состоять только из букв и не превышать длину в 50 символов");
		}

		//Проверка валидности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			newEmployeeName: employeeName);

		if (request == null)
		{
			//Получение заданной команды из базы данных
			Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username)!;

			//Добавление нового пользователя в базу данных
			db.Employees.Add(new()
			{
				Name = employeeName, //Имя нового сотрудника
				Team = team //Команда нового сотрудника
			});

			db.SaveChanges();

			//Ответ в случае успеха, содержит имя команды и имя пользователя
			var response = new
			{
				teamName,
				employeeName
			};

			logger.LogInformation("Add new employee: successfully");

			return Results.Json(response);
		}

		logger.LogError("Add new employee: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}