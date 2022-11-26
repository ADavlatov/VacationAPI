namespace VacationAPI.Entities;

public record Team //Команда пользователя
{
	public Guid Id { get; set; }

	public User User { get; set; } //Пользователь к которому прикреплена команда

	public string Name { get; set; } //Имя команды

	public List<Employee> Employees { get; } = new(); //Сотрудники команды
}