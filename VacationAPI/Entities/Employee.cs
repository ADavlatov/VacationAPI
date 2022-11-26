namespace VacationAPI.Entities;

public record Employee //сотрудник команды
{
	public Guid Id;

	public Team Team { get; set; } //Команда в которой сосотоит сотрудник

	public string Name { get; set; } //Имя сотрудника

	public List<Vacation> Vacations { get; } = new(); //Отпуска сотрудника
}