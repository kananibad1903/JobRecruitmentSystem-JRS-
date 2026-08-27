
using JobRecruitmentSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobRecruitmentSystem.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> Applications { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Employer>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employer)
                .HasForeignKey<Employer>(e => e.UserId);

            modelBuilder.Entity<JobSeeker>()
                .HasOne(js => js.User)
                .WithOne(u => u.JobSeeker)
                .HasForeignKey<JobSeeker>(js => js.UserId);

            modelBuilder.Entity<JobPost>()
                .HasOne(jp => jp.Employer)
                .WithMany(e => e.JobPosts)
                .HasForeignKey(jp => jp.EmployerId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.JobSeeker)
                .WithMany(js => js.Applications)
                .HasForeignKey(a => a.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.JobPost)
                .WithMany(jp => jp.Applications)
                .HasForeignKey(a => a.JobPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedJob>()
                .HasOne(sj => sj.JobSeeker)
                .WithMany(js => js.SavedJobs)
                .HasForeignKey(sj => sj.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedJob>()
                .HasOne(sj => sj.JobPost)
                .WithMany(jp => jp.SavedJobs)
                .HasForeignKey(sj => sj.JobPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedJob>()
                .HasIndex(sj => new { sj.JobSeekerId, sj.JobPostId })
                .IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasIndex(a => new { a.JobSeekerId, a.JobPostId })
                .IsUnique();
        }
    }
}