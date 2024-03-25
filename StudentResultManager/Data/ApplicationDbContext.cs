using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentResultManager.Models;
using System.Reflection.Metadata;

namespace StudentResultManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(e => e.Student)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<Student>(e => e.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Results)
                .HasForeignKey(e => e.StudentId)
                .IsRequired();

            modelBuilder.Entity<Result>()
               .HasOne(r => r.Subject)
               .WithMany(s => s.Results)
               .HasForeignKey(e => e.SubjectId)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
