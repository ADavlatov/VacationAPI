using ImageChartsLib;
using Microsoft.EntityFrameworkCore;
using VacationAPI.Contexts;
using VacationAPI.Entities;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.User.Get.Diagrams;

public class EmployeesOnVacationPerMonth
{
	public static IResult GetDiagram(ApplicationContext db, ILogger logger, string month, string year, string username,
									string accessToken)
	{
		logger.LogInformation("Get diagram with employees on vacation per month: start");

		//проверка данных запроса
		var result = Manager.CheckRequest(db, logger, username, accessToken, month: month,
			year: year);

		if (result == null)
		{
			int daysInCurrentMonth = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));

			Dictionary<int, int> employeesCount = new Dictionary<int, int>();

			//запрос к бд для получения сотрудников и их отпусков
			List<Employee> employees = db.Employees.Where(x => x.Team.User.Name == username)
				.Include(x => x.Vacations).ToList();

			//получение количества сотрудников в отпуске по дням
			for (int i = 1; i != daysInCurrentMonth; i++)
			{
				employeesCount[i] = employees.Count(x => x.Vacations.FirstOrDefault(y =>
															y.StartOfVacation < DateOnly.Parse(i + "." + month + "." + year)
															&& y.EndOfVacation > DateOnly.Parse(i + "." + month + "." + year))
														!= null);
			}

			//получение диаграммы
			string chart = new ImageCharts()
				.cht("bvg") // vertical bar chart
				.chs("700x450") //размер картинки
				.chd("t:" + string.Join(",", employeesCount.Values)) //параметры каждого отдельного столбца
				.chf("b0,lg,90,05B142,1,0CE858,0.2")
				.chxl("0:|" + string.Join("|", Enumerable.Range(1, daysInCurrentMonth)) + "|") // подписи под столбцами
				.chxt("x,y")
				.chtt("Количетсво сотрудников в отпуске на каждый день месяца. " + Helpers.MonthToString.GetString(int.Parse(month)))
				.toURL();

			logger.LogInformation("Get diagram with employees on vacation per month: successfully");

			//ответ в случае успеха
			return Results.Json(chart);
		}

		logger.LogError("Get diagram with employees on vacation per month: failed");

		//ответ в случае ошибки в запросе
		return result;
	}
}