using ImageChartsLib;
using Microsoft.EntityFrameworkCore;
using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Get.Diagrams;

public class EmployeesOnVacationPerYear
{
	public static IResult GetDiagram(ApplicationContext db, ILogger logger, string teamName, string year, string username,
									string accessToken)
	{
		logger.LogInformation("Get diagram with team's employees on vacation per year: start");

		//проверка данных запроса
		var result = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			year: year);

		if (result == null)
		{
			List<int> employeesCount = new List<int>();

			//запрос к бд для получения сотрудников команды и их отпусков
			List<Entities.Employee> employees = db.Employees.Where(x => x.Team.User.Name == username && x.Team.Name == teamName)
				.Include(x => x.Vacations)
				.ToList();

			//получение количества сотрудников в отпуске по месяцам
			for (int i = 0; i < 12; i++)
			{
				employeesCount.Add(employees.Count(x => x.Vacations.FirstOrDefault(y =>
															y.StartOfVacation.Year == int.Parse(year)
															&& y.StartOfVacation.Month == i + 1
															&& y.EndOfVacation.Year == int.Parse(year)
															&& y.EndOfVacation.Month == i + 1)
														!= null));
			}

			//получение диаграммы
			string chart = new ImageCharts()
				.cht("bvg") // vertical bar chart
				.chs("700x450") //размер картинки
				.chd("t:" + string.Join(",", employeesCount)) //параметры каждого отдельного столбца
				.chf("b0,lg,90,05B142,1,0CE858,0.2")
				.chxl("0:|Янв|Февр|Март|Апр|Май|Июнь|Июль|Авг|Сент|Окт|Нояб|Дек|") // подписи под столбцами
				.chxt("x,y")
				.chtt("Количетсво сотрудников команды в отпуске в каждом месяце года.  " + year + " Команда: " + teamName)
				.toURL();

			//ответ в случае успеха
			return Results.Json(chart);
		}

		//ответ в случае ошибки в запросе
		return result;
	}
}