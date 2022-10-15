namespace VacationAPI.Entities;

public record User()
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }

	public List<Team> Teams { get; set; } = new();
}