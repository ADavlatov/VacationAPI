using VacationAPI.Context;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;
using Team = VacationAPI.Entities.Team;

namespace VacationAPI.Request.Employees.Post;

public class Employee
{
	public static IResult AddNewEmployee(ApplicationContext db, EmployeeValidator employeeValidator, ILogger logger, string teamName,
										string employeeName,
										string username,
										string accessToken)
	{
		logger.LogInformation("Add new employee: start");

		if (!employeeValidator.Validate(new Services.Validation.Employee(employeeName))
				.IsValid)
		{
			logger.LogError("Add new employee: wrong format of input");

			Results.Json("Недопустимый формат ввода. Имя сотрудника должно состоять только из букв и не превышать длину в 50 символов");
		}

		var request = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			newEmployeeName: employeeName);

		Team team = db.Teams.FirstOrDefault(x => x.Name == teamName && x.User.Name == username)!;

		if (request == null)
		{
			db.Employees.Add(new()
			{
				Name = employeeName,
				Team = team
			});

			db.SaveChanges();

			var response = new
			{
				teamName,
				employeeName,
			};

			logger.LogInformation("Add new employee: successfully");

			return Results.Json(response);
		}

		logger.LogError("Add new employee: failed");

		return request;
	}
}