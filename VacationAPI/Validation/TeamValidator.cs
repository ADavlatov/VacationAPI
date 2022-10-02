using FluentValidation;
using VacationAPI.Context;
using VacationAPI.Validation.ValidationParameters;

namespace VacationAPI.Validation;

public class TeamValidator : AbstractValidator<TeamNameParameter>
{
	public TeamValidator(ApplicationContext db)
	{
		var msgTeamName = "Ошибка в поле teamName: команда с таким именем не найдена";

		RuleFor(x => x.TeamName)
			.Must(x => db.Teams.FirstOrDefault(j => j.Name == x)
						!= null)
			.WithMessage(msgTeamName);
	}
}