namespace VacationAPI.Entities;

public record Employee()
{
    public Guid Id;
	public Team Team { get; set; }
	public string Name { get; set; }
	public string Position { get; set; }
	public List<Vacation> Vacations { get; set; }
}