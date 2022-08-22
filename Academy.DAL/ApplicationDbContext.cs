using Academy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academy.DAL;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Course> Course { get; set; } = null!;
    public DbSet<Group> Group { get; set; } = null!;
    public DbSet<Student> Student { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasMaxLength(50).IsRequired();
            entity.Property(c => c.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).ValueGeneratedOnAdd();
            entity.Property(g => g.Name).HasMaxLength(50).IsRequired();
            entity.HasOne(c => c.Course).WithMany(g => g.Groups).HasForeignKey(g => g.CourseId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.Property(s => s.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(s => s.LastName).HasMaxLength(50).IsRequired();
            entity.Property(s => s.CourseId).IsRequired();
            entity.Property(s => s.GroupId).IsRequired();
            entity.HasOne(g => g.Group).WithMany(s => s.Students).HasForeignKey(s => s.GroupId);
        });

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Name = "First", Description = "Description of First course." },
            new Course { Id = 2, Name = "Second", Description = "Description of Second course." },
            new Course { Id = 3, Name = "Third", Description = "Description of Third course." });

        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, Name = "A1 gr.", CourseId = 1 },
            new Group { Id = 2, Name = "B1 gr.", CourseId = 1 },
            new Group { Id = 3, Name = "A2 gr.", CourseId = 2 },
            new Group { Id = 4, Name = "B2 gr.", CourseId = 2 },
            new Group { Id = 5, Name = "C2 gr.", CourseId = 2 },
            new Group { Id = 6, Name = "A3 gr.", CourseId = 3 },
            new Group { Id = 7, Name = "B3 gr.", CourseId = 3 });

        modelBuilder.Entity<Student>().HasData(
            new Student{ Id = 1, FirstName = "Sam", LastName = "One", GroupId = 1, CourseId = 1},
            new Student { Id = 2, FirstName = "Jon", LastName = "Two", GroupId = 1, CourseId = 1 },
            new Student { Id = 3, FirstName = "Din", LastName = "Three", GroupId = 1, CourseId = 1 },
            new Student { Id = 4, FirstName = "Max", LastName = "Four", GroupId = 2, CourseId = 1 },
            new Student { Id = 5, FirstName = "Frank", LastName = "Five", GroupId = 2, CourseId = 1 },
            new Student { Id = 6, FirstName = "Roma", LastName = "Six", GroupId = 3, CourseId = 2 },
            new Student { Id = 7, FirstName = "Cris", LastName = "Seven", GroupId = 3, CourseId = 2 },
            new Student { Id = 8, FirstName = "Lord", LastName = "Eight", GroupId = 3, CourseId = 2 },
            new Student { Id = 9, FirstName = "Peter", LastName = "Nine", GroupId = 4, CourseId = 2 },
            new Student { Id = 10, FirstName = "Peter", LastName = "Ten", GroupId = 4, CourseId = 2 },
            new Student { Id = 11, FirstName = "Ivan", LastName = "Eleven", GroupId = 6, CourseId = 3 },
            new Student { Id = 12, FirstName = "Tom", LastName = "Twelve", GroupId = 6, CourseId = 3 },
            new Student { Id = 13, FirstName = "Li", LastName = "Thirteen", GroupId = 6, CourseId = 3 },
            new Student { Id = 14, FirstName = "Udo", LastName = "Fourteen", GroupId = 7, CourseId = 3 });
    }
}