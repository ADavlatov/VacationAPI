using ImageChartsLib;
using VacationAPI.Contexts;
using VacationAPI.Services.RequestServices;

namespace VacationAPI.Requests.Teams.Get.Diagrams;

public class EmployeesOnVacationDaysCount
{
	public static IResult GetDiagramPerYear(ApplicationContext db, ILogger logger, string teamName, string year, string username,
											string accessToken)
	{
		logger.LogInformation("Get diagram with employee's days on vacation count per year: start");

		//Проверка валдиности данных запроса
		var result = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			year: year);

		if (result == null)
		{
			Dictionary<string, int> employees = new Dictionary<string, int>();

			//плдучение списка сотрудников команды
			foreach (var employee in db.Employees.Where(x => x.Team.User.Name == username && x.Team.Name == teamName))
			{
				employees[employee.Name] = 0;
			}

			//получение количества дней в отпуске каждого сотрудника
			foreach (var vacation in db.Vacations.Where(x =>
						x.Employee != null
						&& x.Employee.Team.Name == teamName
						&& x.Employee.Team.User.Name == username
						&& x.StartOfVacation.Year == int.Parse(year)
						&& x.EndOfVacation.Year == int.Parse(year)))
			{
				if ((int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0))).TotalDays == 0)
				{
					employees[vacation.Employee!.Name] += 1;
				} else
				{
					employees[vacation.Employee!.Name] +=
						(int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0))).TotalDays;
				}
			}

			//получение диаграммы
			string chart = new ImageCharts()
				.cht("bhg") // horizontal bar chart
				.chs("700x450") //размер картинки
				.chd("t:" + string.Join(",", employees.Values)) //параметры каждого отдельного столбца
				.chf("b0,lg,90,05B142,1,0CE858,0.2")
				.chxl("1:|" + string.Join("|", employees.Keys.Reverse()) + "|") // подписи под столбцами
				.chxt("x,y")
				.chtt("Количетсво отпусков сотрудников в днях за год. " + year)
				.toURL();

			logger.LogInformation("Get diagram with employee's days on vacation count per year: successfully");

			//ответ в случае успеха
			return Results.Json(chart);
		}

		logger.LogError("Get diagram with employee's days on vacation count per year: failed");

		//ответ в случае ошибки в заросе, выявленной в классе Manager
		return result;
	}

	public static IResult GetDiagramPerMonth(ApplicationContext db, ILogger logger, string teamName, string month, string year,
											string username,
											string accessToken)
	{
		logger.LogInformation("Get diagram with employee's days on vacation count per month: start");

		//Проверка валдиности данных запроса
		var result = Manager.CheckRequest(db, logger, username, accessToken, teamName: teamName,
			month: month,
			year: year);

		if (result == null)
		{
			Dictionary<string, int> employees = new Dictionary<string, int>();

			//плдучение списка сотрудников команды
			foreach (var employee in db.Employees.Where(x => x.Team.User.Name == username && x.Team.Name == teamName))
			{
				employees[employee.Name] = 0;
			}

			//получение количества дней в отпуске каждого сотрудника
			foreach (var vacation in db.Vacations.Where(x =>
						x.Employee != null
						&& x.Employee.Team.Name == teamName
						&& x.Employee.Team.User.Name == username
						&& x.StartOfVacation.Year == int.Parse(year)
						&& x.StartOfVacation.Month == int.Parse(month)
						&& x.EndOfVacation.Year == int.Parse(year)
						&& x.EndOfVacation.Month == int.Parse(month)))
			{
				if ((int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0))).TotalDays == 0)
				{
					employees[vacation.Employee!.Name] += 1;
				} else
				{
					employees[vacation.Employee!.Name] +=
						(int) (vacation.EndOfVacation.ToDateTime(new(0, 0)) - vacation.StartOfVacation.ToDateTime(new(0, 0))).TotalDays;
				}
			}

			//получение диаграммы
			string chart = new ImageCharts()
				.cht("bhg") // horizontal bar chart
				.chs("700x450") //размер картинки
				.chd("t:" + string.Join(",", employees.Values)) //параметры каждого отдельного столбца
				.chf("b0,lg,90,05B142,1,0CE858,0.2")
				.chxl("1:|" + string.Join("|", employees.Keys.Reverse()) + "|") // подписи под столбцами
				.chxt("x,y")
				.chtt("Количетсво отпусков сотрудников в днях за год. " + year)
				.toURL();

			logger.LogInformation("Get diagram with employee's days on vacation count per month: successfully");

			//ответ в случае успеха
			return Results.Json(chart);
		}

		logger.LogError("Get diagram with employee's days on vacation count per month: failed");

		//ответ в случае ошибки в заросе
		return result;
	}
}