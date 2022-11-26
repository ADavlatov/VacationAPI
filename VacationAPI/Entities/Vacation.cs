namespace VacationAPI.Entities;

public record Vacation //Отпуск сотрудника
{
	public Guid Id { get; set; }

	public Employee? Employee { get; set; } //Сотрудник к которому прикрепляется отпуск

	public DateOnly StartOfVacation { get; set; } //Начало отпуска

	public DateOnly EndOfVacation { get; set; } //Конец отпуска
}