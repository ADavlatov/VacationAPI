using FluentValidation;
using VacationAPI.Context;
using VacationAPI.Validation.ValidationParameters;

namespace VacationAPI.Validation;

public class EmployeeValidator : AbstractValidator<EmployeeNameParameter>
{
	public EmployeeValidator(ApplicationContext db)
	{
		var msgEmployeeName = "Ошибка в поле employeeName: сотрудник с таким именем не найден";

		RuleFor(x => x.EmployeeName)
			.Must(x => db.Employees.FirstOrDefault(j => j.Name == x)
						!= null)
			.WithMessage(msgEmployeeName);
	}
}