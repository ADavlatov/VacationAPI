namespace VacationAPI.Entities;

public record Vacation
{
	public Guid Id { get; set; }

	public Employee? Employee { get; set; }

	public DateOnly StartOfVacation { get; set; }

	public DateOnly EndOfVacation { get; set; }
}