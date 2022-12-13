using ImageChartsLib;
using Microsoft.EntityFrameworkCore;
using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get.Diagrams;

public class EmployeesOnVacation
{
	public static IResult GetDiagram(ApplicationContext db, ILogger logger, string username, string accessToken)
	{
		logger.LogInformation("Get diagram with employees on vacation count: started");

		//проверка данных запроса
		var result = Manager.CheckRequest(db, logger, username, accessToken);

		if (result == null)
		{
			Dictionary<string, int> teamsAndEmployees = new Dictionary<string, int>();

			//получение количества сотрудников в отпуске в каждой команде
			foreach (var team in db.Teams.Where(x => x.User.Name == username)
						.Include(x => x.Employees)
						.ThenInclude(x => x.Vacations))
			{
				teamsAndEmployees[team.Name] = team.Employees.Count(x => x.Vacations.FirstOrDefault(y =>
																			y.StartOfVacation <= DateOnly.FromDateTime(DateTime.Now)
																			&& y.EndOfVacation >= DateOnly.FromDateTime(DateTime.Now))
																		!= null);
			}

			//получение диаграммы
			string chart = new ImageCharts()
				.cht("bvg") // vertical bar chart
				.chs("700x450") //размер картинки
				.chd("t:" + string.Join(",", teamsAndEmployees.Values)) //параметры каждого отдельного столбца
				.chf("b0,lg,90,05B142,1,0CE858,0.2")
				.chxl("0:|" + string.Join("|", teamsAndEmployees.Keys) + "|") // подписи под столбцами
				.chxt("x,y")
				.chtt("Количетсво сотрудников в отпуске в каждой команде на момент " + DateTime.Now.Date)
				.toURL();

			logger.LogInformation("Get diagram with employees on vacation count: successfully");

			//ответ в случае успеха
			return Results.Json(chart);
		}

		logger.LogError("Get diagram with employees on vacation count: failed");

		//ответ в случае ошибки в запросе
		return result;
	}
}