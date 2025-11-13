
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ITI_APP.Models
{
    public class ITIEntities : IdentityDbContext<ApplicationUser>
    {
        public ITIEntities() { }
        public ITIEntities(DbContextOptions options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CrsResults> CrsResults { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.DeptId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Course)
                .WithMany(c => c.Instructors)
                .HasForeignKey(i => i.CrsId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CrsResults>()
                .HasOne(cr => cr.Student)
                .WithMany(t => t.CrsResults)
                .HasForeignKey(cr => cr.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CrsResults>()
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CrsResults)
                .HasForeignKey(cr => cr.CrsId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CrsResults>()
                .HasKey(cr => new { cr.StudentId, cr.CrsId });

        }
    }
}
