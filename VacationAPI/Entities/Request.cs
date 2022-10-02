namespace VacationAPI.Entities;

public class Request
{
	public string Username { get; set; }
	public string AccessToken { get; set; }
	public string? TeamName { get; set; }
	public string? EmployeeName { get; set; }
	public string? VacationDateStart { get; set; }
	public string? VacationDateEnd { get; set; }
}