namespace VacationAPI.Entities;

public record User //Пользователь
{
	public Guid Id { get; set; }

	public string Name { get; set; } //Имя пользователя

	public string Password { get; set; } //Пароль пользователя (Хэшируется MD5)

	public List<Team> Teams { get; set; } = new(); //Команды пользователя
}