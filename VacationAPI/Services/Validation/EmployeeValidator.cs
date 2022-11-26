using FluentValidation;

namespace VacationAPI.Services.Validation;

public class EmployeeValidator : AbstractValidator<Employee>
{
	public EmployeeValidator()
	{
		RuleFor(x => x.EmployeeName)
			.Must(x => x.All(char.IsLetter))
			.Must(x => x.Length < 50);
	}
}

public class Employee
{
	public Employee(string employeeName)
	{
		EmployeeName = employeeName;
	}

	public string EmployeeName { get; }
}