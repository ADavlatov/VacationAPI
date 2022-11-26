using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;

namespace VacationAPI.Request.Employees.Put;

public class EmployeeName
{
	//Смена имени сотрудника команды
	public static IResult EditEmployeeName(ApplicationContext db, EmployeeValidator employeeValidator, ILogger logger, string teamName,
											string oldEmployeeName, string newEmployeeName,
											string username,
											string accessToken)
	{
		logger.LogInformation("Change employee name: start");

		//Проверка имени сотрудника на валидность
		if (!employeeValidator.Validate(new Employee(newEmployeeName))
				.IsValid)
		{
			logger.LogError("Change employee name: wrong format of input");

			//Ответ в случае ошибки в имени сотрудника
			return Results.Json("Недопустимый формат ввода. Имя сотрудника должно состоять только из букв и не превышать длину в 50 символов");
		}

		//Проверка валдиности данных в запросе
		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			employeeName: oldEmployeeName,
			newEmployeeName: newEmployeeName);

		if (request == null)
		{
			//Получение сотрудника для изменения имени
			Entities.Employee employee =
				db.Employees.FirstOrDefault(x => x.Name == oldEmployeeName && x.Team.Name == teamName && x.Team.User.Name == username)!;

			//Изменение имени сотруднику
			employee.Name = newEmployeeName;
			db.SaveChanges();

			//Ответ пользователю в случае успеха, содержит имя команды сотрудника, старое и новое имя сотруднка
			var response = new
			{
				teamName,
				oldEmployeeName,
				newEmployeeName
			};

			logger.LogInformation("Change employee name: successfully");

			return Results.Json(response);
		}

		logger.LogError("Change employee name: failed");

		//Ответ в случае ошибки в запросе, выявленной в классе Manager
		return request;
	}
}