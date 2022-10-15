namespace VacationAPI.Entities;

public record Team()
{
	public Guid Id { get; set; }

	public User User { get; set; }

	public string Name { get; set; }

	public List<Employee> Employees { get; set; } = new();
}