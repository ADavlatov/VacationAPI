using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationAPI.Entities;

namespace VacationAPI.Context;

public class ApplicationContext : DbContext
{
	public DbSet<Team> Teams => Set<Team>();
	public DbSet<Employee> Employees => Set<Employee>();
	public DbSet<Vacation> Vacations => Set<Vacation>();

	public ApplicationContext()
	{
		Database.EnsureCreated();
	}


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=Database/Users.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new TeamConfiguration());
		modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
		modelBuilder.ApplyConfiguration(new VacationConfiguration());
	}
}

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
	public void Configure(EntityTypeBuilder<Team> builder)
	{
		builder.HasKey(x => x.Id);
	}
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
	public void Configure(EntityTypeBuilder<Employee> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.Team).WithMany(x => x.Employees);
	}
}
public class VacationConfiguration : IEntityTypeConfiguration<Vacation>
{
	public void Configure(EntityTypeBuilder<Vacation> builder)
	{
		builder.HasNoKey();
		builder.HasOne(x => x.Employee).WithMany(x => x.Vacations);
	}
}