using JobPortal.Public.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Public.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Applicant> Applicants { get; set; }
    public DbSet<EducationDetail> EducationDetails { get; set; }
    public DbSet<ExperienceDetail> ExperienceDetails { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobPosition>().HasData(
            new JobPosition
            {
                JobPositionId = 1, PositionName = "Software Engineer",
                Description = "Develop and maintain software applications."
            },
            new JobPosition
            {
                JobPositionId = 2, PositionName = "Data Analyst",
                Description = "Analyze data and generate reports for business decisions."
            },
            new JobPosition
            {
                JobPositionId = 3, PositionName = "Product Manager",
                Description = "Oversee the development and launch of products."
            },
            new JobPosition
            {
                JobPositionId = 4, PositionName = "DevOps Engineer",
                Description = "Maintain and enhance CI/CD pipelines and cloud infrastructure."
            },
            new JobPosition
            {
                JobPositionId = 5, PositionName = "UI/UX Designer",
                Description = "Design user interfaces and improve user experience."
            }
        );

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.EducationDetails)
            .WithOne(e => e.Applicant)
            .HasForeignKey(e => e.ApplicantId);

        modelBuilder.Entity<Applicant>()
            .HasMany(a => a.ExperienceDetails)
            .WithOne(e => e.Applicant)
            .HasForeignKey(e => e.ApplicantId);

        modelBuilder.Entity<Applicant>()
            .HasOne(a => a.JobPosition)
            .WithMany(j => j.Applicants)
            .HasForeignKey(a => a.JobPositionId);
    }
}