namespace VacationAPI.Entities;

public record Employee()
{
	public Guid Id;

	public Team Team { get; set; }

	public string Name { get; set; }

	public List<Vacation> Vacations { get; } = new();
}