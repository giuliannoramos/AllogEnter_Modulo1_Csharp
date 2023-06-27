using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Univali.Api.Entities;

namespace Univali.Api.DbContexts;

public class PublisherContext : DbContext
{
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=UnivaliTrabalho;Username=postgres;Password=1973"
        ).LogTo(Console.WriteLine,
        new[] { DbLoggerCategory.Database.Command.Name },
        LogLevel.Information)
        .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)// exemplo de FluentApi
    {
        var author = modelBuilder.Entity<Author>();

        author
        .Property(a => a.FirstName)
        .HasMaxLength(30)
        .IsRequired();

        author
        .Property(a => a.LastName)
        .HasMaxLength(30)
        .IsRequired();

        var course = modelBuilder.Entity<Course>();

        course
        .Property(c => c.Title)
        .HasMaxLength(60)
        .IsRequired();

        course
        .Property(c => c.Price)
        .HasPrecision(5, 2)
        .HasColumnName("BasePrice")//não fazer aqui, alterar nome na direto na entidade por boa prática
        .IsRequired();

        course
        .Property(c => c.Description)
        .IsRequired(false);

    }
}