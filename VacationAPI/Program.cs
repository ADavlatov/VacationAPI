using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using VacationAPI.Context;
using VacationAPI.Request.Authentication;
using VacationAPI.Services.RequestServices;
using VacationAPI.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new()
	{
		Title = "VacationAPI",
		Description = "An ASP.NET Core Web API for managing vacations",
		Contact = new()
		{
			Name = "GitHub",
			Url = new("https://github.com/ADavlatov/VacationAPI")
		}
	});
});
builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<UserValidator>();
builder.Services.AddScoped<TeamValidator>();
builder.Services.AddScoped<EmployeeValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new()
		{
			ValidateIssuer = true,
			ValidIssuer = AuthOptions.Issuer,
			ValidateAudience = true,
			ValidAudience = AuthOptions.Audience,
			ValidateLifetime = true,
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

new RequestRouter().Route(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();