using FluentValidation;

namespace VacationAPI.Services.Validation;

public class TeamValidator : AbstractValidator<Team>
{
	public TeamValidator()
	{
		RuleFor(x => x.TeamName)
			.Must(x => x.All(char.IsLetter))
			.Must(x => x.Length < 50);
	}
}

public class Team
{
	public Team(string teamName)
	{
		TeamName = teamName;
	}

	public string TeamName { get; }
}