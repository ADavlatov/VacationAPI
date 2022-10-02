using FluentValidation;
using VacationAPI.Entities;
using VacationAPI.Validation.ValidationParameters;

namespace VacationAPI.Validation;

public class DateValidator : AbstractValidator<DateParameter>
{
	public DateValidator()
	{
		var msgDate = "Ошибка в поле vacationDate: неправильный формат ввода даты";
		DateOnly date;

		RuleFor(x => x.VacationDate)
			.Must(x => DateOnly.TryParse(x, out date))
			.WithMessage(msgDate);
	}
}