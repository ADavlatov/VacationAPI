namespace VacationAPI.Entities;

public record Vacation()
{
	public Employee? Employee { get; set; }
	public DateOnly StartOfVacation { get; set; }
	public DateOnly EndOfVacation { get; set; }
}